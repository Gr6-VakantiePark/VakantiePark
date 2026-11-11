using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Infrastructure.Data.Seeders;

namespace VakantiePark.Infrastructure.Data
{
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
    {
        public DbSet<Accommodatie> Accommodaties => Set<Accommodatie>();
        public DbSet<AccommodatieType> AccommodatieTypes => Set<AccommodatieType>();
        public DbSet<Voorziening> Voorzieningen => Set<Voorziening>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Laadt alle IEntityTypeConfiguration-klassen uit Data/Configurations
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Draait automatisch bij Database.Migrate() / EnsureCreated() tijdens het opstarten
            optionsBuilder.UseSeeding((context, _) =>
            {
                context.UseAccommodatieSeeder();
            });

            // Async variant (voor MigrateAsync / EnsureCreatedAsync), doet hetzelfde
            optionsBuilder.UseAsyncSeeding((context, _, _) =>
            {
                context.UseAccommodatieSeeder();
                return Task.CompletedTask;
            });

            base.OnConfiguring(optionsBuilder);
        }
    }
}
