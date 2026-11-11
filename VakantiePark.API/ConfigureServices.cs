using Microsoft.AspNetCore.Mvc;
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

            return services;
        }

    }
}
