using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class AccommodationConfiguration : IEntityTypeConfiguration<Accommodation>
    {
        public void Configure(EntityTypeBuilder<Accommodation> builder)
        {
            builder.ToTable("Accommodations");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Code).IsRequired().HasMaxLength(20);
            builder.HasIndex(a => a.Code).IsUnique();

            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Zone).IsRequired().HasMaxLength(50);

            builder.Property(a => a.MaxVehicleLengthMeters).HasPrecision(4, 1);
            builder.Property(a => a.MaxCaravanLengthMeters).HasPrecision(4, 1);

            builder.Property(a => a.Surface).HasMaxLength(30);
            builder.Property(a => a.Shade).HasMaxLength(20);
            builder.Property(a => a.WifiCoverage).HasMaxLength(20);

            builder.HasMany(a => a.Amenities)
                .WithMany(v => v.Accommodations)
                .UsingEntity(j => j.ToTable("AccommodationAmenities"));
        }
    }
}
