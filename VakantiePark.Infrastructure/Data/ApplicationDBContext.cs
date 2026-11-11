using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Infrastructure.Data.Seeders;

namespace VakantiePark.Infrastructure.Data
{
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
    {
        public DbSet<Accommodation> Accommodations => Set<Accommodation>();
        public DbSet<AccommodationType> AccommodationTypes => Set<AccommodationType>();
        public DbSet<Amenity> Amenities => Set<Amenity>();
        public DbSet<Photo> Photos => Set<Photo>();
        public DbSet<PhotoFile> PhotoFiles => Set<PhotoFile>();
        public DbSet<ParkSettings> ParkSettings => Set<ParkSettings>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSeeding((context, _) =>
            {
                context.UseAccommodationSeeder();
                context.UsePriceSeeder();
            });

            optionsBuilder.UseAsyncSeeding((context, _, _) =>
            {
                context.UseAccommodationSeeder();
                context.UsePriceSeeder();
                return Task.CompletedTask;
            });

            base.OnConfiguring(optionsBuilder);
        }
    }
}
