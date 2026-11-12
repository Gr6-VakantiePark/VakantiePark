using System.Security.Cryptography;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using VakantiePark.Application.Common;
using VakantiePark.Application.Interfaces;
using VakantiePark.Infrastructure.Security;
using VakantiePark.Shared.Common;

namespace VakantiePark.API
{
    public static class ConfigureServices
    {
        public const int RequestsPerMinute = 50;

        public static IServiceCollection RegisterAPIServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(environment);

            services.RegisterControllers();
            services.RegisterSwagger();
            services.RegisterCors(configuration);
            services.RegisterAuthentication(configuration, environment);
            services.RegisterRateLimiter();
            services.RegisterExceptionHandler();

            return services;
        }

        private static IServiceCollection RegisterControllers(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddControllers()
                .AddJsonOptions(options => JsonDefaults.Configure(options.JsonSerializerOptions));
            services.AddEndpointsApiExplorer();

            services.PostConfigure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    string? error = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .Select(e => $"Ongeldige waarde voor '{e.Key}'.")
                        .FirstOrDefault();

                    return new BadRequestObjectResult(ApiResponse<object>.Fail(error ?? ApiMessages.InvalidRequest));
                };
            });

            return services;
        }

        private static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "VakantiePark API",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter your JWT token"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                });
            });

            return services;
        }

        private static IServiceCollection RegisterCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<CorsSettings>()
                .Bind(configuration.GetSection(CorsSettings.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddCors();
            services.AddOptions<CorsOptions>()
                .Configure<IOptions<CorsSettings>>((options, settings) =>
                    options.AddPolicy(CorsSettings.PolicyName, policy =>
                        policy.WithOrigins(settings.Value.AllowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()));

            return services;
        }

        private static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection(JwtSettings.SectionName))
                .PostConfigure(settings =>
                {
                    if (string.IsNullOrWhiteSpace(settings.Key) && environment.IsDevelopment())
                        settings.Key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<JwtSettings>>((options, jwtOptions) =>
                {
                    var settings = jwtOptions.Value;

                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = settings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = settings.EffectiveAudience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = JwtTokenService.CreateSigningKey(settings),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),
                        NameClaimType = JwtRegisteredClaimNames.Name,
                        RoleClaimType = "role",
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var repository = context.HttpContext.RequestServices.GetRequiredService<IEmployeeRepository>();
                            if (context.Principal is null
                                || !context.Principal.TryGetUserId(out Guid userId)
                                || !await repository.IsActiveAsync(userId))
                            {
                                context.Fail(ApiMessages.NotLoggedIn);
                            }
                        },
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            await WriteFailureAsync(context.Response, StatusCodes.Status401Unauthorized, ApiMessages.NotLoggedIn);
                        },
                        OnForbidden = context =>
                            WriteFailureAsync(context.Response, StatusCodes.Status403Forbidden, ApiMessages.Forbidden),
                    };
                });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                foreach (string permission in Permissions.All)
                    options.AddPolicy(permission, policy => policy.RequireClaim(Permissions.ClaimType, permission));
            });

            return services;
        }

        private static IServiceCollection RegisterRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    string partition = context.User.TryGetUserId(out Guid userId)
                        ? $"user:{userId}"
                        : $"ip:{context.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";

                    return RateLimitPartition.GetFixedWindowLimiter(partition, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = RequestsPerMinute,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                    });
                });

                options.OnRejected = (context, _) =>
                    new ValueTask(WriteFailureAsync(context.HttpContext.Response, StatusCodes.Status429TooManyRequests, ApiMessages.TooManyRequests));
            });

            return services;
        }

        private static IServiceCollection RegisterExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }

        private static Task WriteFailureAsync(HttpResponse response, int statusCode, string message)
        {
            response.StatusCode = statusCode;
            return response.WriteAsJsonAsync(ApiResponse<object>.Fail(message), JsonDefaults.Options);
        }
    }
}
