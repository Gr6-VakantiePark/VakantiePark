using Microsoft.Extensions.DependencyInjection;
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


            return services;
        }
    }
}
