using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using VakantiePark.Application.Common;
using VakantiePark.Infrastructure.Security;
using VakantiePark.Shared.Common;

namespace VakantiePark.API
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterAPIServices(this IServiceCollection services, IConfiguration configuration)
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

            services.RegisterAuthentication(configuration);

            return services;
        }

        private static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                ?? throw new InvalidOperationException("De JWT-instellingen ontbreken in appsettings.json.");
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
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            await WriteFailureAsync(context.Response, StatusCodes.Status401Unauthorized, ApiMessages.NotLoggedIn);
                        },
                        OnForbidden = context =>
                            WriteFailureAsync(context.Response, StatusCodes.Status403Forbidden, ApiMessages.Forbidden),
                    };
                });

            services.AddAuthorization();

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
