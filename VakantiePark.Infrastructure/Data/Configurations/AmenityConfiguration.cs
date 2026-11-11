using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
    {
        public void Configure(EntityTypeBuilder<Amenity> builder)
        {
            builder.ToTable("Amenities");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(v => v.Code).IsUnique();

            builder.Property(v => v.Name).IsRequired().HasMaxLength(100);
        }
    }
}
