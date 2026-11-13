using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class AccommodationTypeConfiguration : IEntityTypeConfiguration<AccommodationType>
    {
        public void Configure(EntityTypeBuilder<AccommodationType> builder)
        {
            builder.ToTable("AccommodationTypes");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(t => t.Name).IsUnique();

            builder.Property(t => t.Slug).IsRequired().HasMaxLength(100);
            builder.HasIndex(t => t.Slug).IsUnique();

            builder.Property(t => t.Category).IsRequired().HasConversion<string>().HasMaxLength(20);

            builder.Property(t => t.Tagline).HasMaxLength(200);
            builder.Property(t => t.Description).HasMaxLength(2000);
            builder.Property(t => t.PricePerNight).HasPrecision(10, 2);

            builder.HasMany(t => t.Accommodations)
                .WithOne(a => a.AccommodationType)
                .HasForeignKey(a => a.AccommodationTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
