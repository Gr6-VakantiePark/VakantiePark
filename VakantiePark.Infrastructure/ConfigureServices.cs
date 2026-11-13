

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Infrastructure.Data;
using VakantiePark.Infrastructure.Repositories;
using VakantiePark.Infrastructure.Security;

namespace VakantiePark.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration, string root)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            services.RegisterDatabaseServices(configuration, root);
            services.AddScoped<IAccommodationRepository, AccommodationRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IParkSettingsRepository, ParkSettingsRepository>();
            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
            services.AddSingleton<ITokenService, JwtTokenService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IUnrentablePeriodRepository, UnrentablePeriodRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<ITransactionRunner, TransactionRunner>();


            return services;
        }

        public static IServiceCollection RegisterDatabaseServices(this IServiceCollection services, IConfiguration configuration, string root)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentException.ThrowIfNullOrWhiteSpace(root);

            services.AddOptions<DatabaseSettings>()
                .Bind(configuration)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddDbContext<ApplicationDBContext>((provider, options) =>
            {
                var settings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                options.UseSqlite(settings.CreateConnectionString(root));
            });

            return services;
        }
    }
}
