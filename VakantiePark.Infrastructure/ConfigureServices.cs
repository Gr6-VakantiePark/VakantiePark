

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VakantiePark.Application.Interfaces;
using VakantiePark.Infrastructure.Data;
using VakantiePark.Infrastructure.Repositories;
using VakantiePark.Infrastructure.Security;

namespace VakantiePark.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration, string root)
        {
            services.RegisterDatabaseServices(configuration, root);
            services.AddScoped<IAccommodationRepository, AccommodationRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IParkSettingsRepository, ParkSettingsRepository>();
            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
            services.AddSingleton<ITokenService, JwtTokenService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();


            return services;
        }

        public static IServiceCollection RegisterDatabaseServices(this IServiceCollection services, IConfiguration configuration, string root)
        {
            string? configuredFolder = configuration["DataFolder"];
            string folder = string.IsNullOrWhiteSpace(configuredFolder)
                ? Path.GetFullPath(Path.Combine(root, "..", "VakantiePark.Infrastructure", "Data"))
                : Path.GetFullPath(configuredFolder);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string dataSource = configuration["DataSource"] ?? "app.db";
            string connectionString = $"Data Source={Path.Combine(folder, dataSource)}";

            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }
    }
}
