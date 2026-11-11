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
        public DbSet<Afbeelding> Afbeeldingen => Set<Afbeelding>();
        public DbSet<AfbeeldingBestand> AfbeeldingBestanden => Set<AfbeeldingBestand>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSeeding((context, _) =>
            {
                context.UseAccommodatieSeeder();
            });

            optionsBuilder.UseAsyncSeeding((context, _, _) =>
            {
                context.UseAccommodatieSeeder();
                return Task.CompletedTask;
            });

            base.OnConfiguring(optionsBuilder);
        }
    }
}
