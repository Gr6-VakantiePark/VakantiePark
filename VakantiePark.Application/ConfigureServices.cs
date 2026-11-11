using Microsoft.Extensions.DependencyInjection;
using VakantiePark.Application.Interfaces;
using VakantiePark.Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace VakantiePark.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.RegisterScopedServices();

            return services;
        }
        public static IServiceCollection RegisterScopedServices(this IServiceCollection services)
        {
            services.AddScoped<IAccommodationService, AccommodationService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IPriceService, PriceService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            return services;
        }
    }
}
