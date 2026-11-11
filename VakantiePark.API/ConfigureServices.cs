using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using VakantiePark.Application.Common;
using VakantiePark.Application.Interfaces;
using VakantiePark.Infrastructure.Security;
using VakantiePark.Shared.Common;

namespace VakantiePark.API
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterAPIServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
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

            services.RegisterAuthentication(configuration, environment);

            return services;
        }

        private static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var settings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                ?? throw new InvalidOperationException("De JWT-instellingen ontbreken in appsettings.json.");

            if (string.IsNullOrWhiteSpace(settings.Key) && environment.IsDevelopment())
                settings.Key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            services.AddSingleton(settings);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
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

        private static Task WriteFailureAsync(HttpResponse response, int statusCode, string message)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json";
            return response.WriteAsync(JsonSerializer.Serialize(ApiResponse<object>.Fail(message), JsonSerializerOptions.Web));
        }
    }
}
