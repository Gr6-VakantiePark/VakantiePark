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
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();

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
                context.UseRoleSeeder();
                context.UseEmployeeSeeder();
            });

            optionsBuilder.UseAsyncSeeding((context, _, _) =>
            {
                context.UseAccommodationSeeder();
                context.UseRoleSeeder();
                context.UseEmployeeSeeder();
                return Task.CompletedTask;
            });

            base.OnConfiguring(optionsBuilder);
        }
    }
}
