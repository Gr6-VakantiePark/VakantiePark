using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class ParkSettingsConfiguration : IEntityTypeConfiguration<ParkSettings>
    {
        public void Configure(EntityTypeBuilder<ParkSettings> builder)
        {
            builder.ToTable("ParkSettings");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.CleaningFee).HasPrecision(10, 2);
            builder.Property(i => i.TouristTaxPerPersonPerNight).HasPrecision(10, 2);
            builder.Property(i => i.TouristTaxMinimumAge).IsRequired();
            builder.Property(i => i.WeekendSurchargePercentage).HasPrecision(5, 2);
            builder.Property(i => i.PetFeePerNight).HasPrecision(10, 2);
            builder.Property(i => i.ArrivalTime).IsRequired();
            builder.Property(i => i.DepartureTime).IsRequired();
            builder.Property(i => i.MinimumNightsHighSeason).IsRequired();
            builder.Property(i => i.Street).IsRequired().HasMaxLength(100);
            builder.Property(i => i.PostalCode).IsRequired().HasMaxLength(10);
            builder.Property(i => i.City).IsRequired().HasMaxLength(100);
            builder.Property(i => i.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(i => i.Email).IsRequired().HasMaxLength(254);
        }
    }
}
