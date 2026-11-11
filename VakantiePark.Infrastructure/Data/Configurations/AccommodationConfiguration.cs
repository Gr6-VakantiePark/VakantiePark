using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class AccommodatieConfiguration : IEntityTypeConfiguration<Accommodatie>
    {
        public void Configure(EntityTypeBuilder<Accommodatie> builder)
        {
            builder.ToTable("Accommodaties");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Code).IsRequired().HasMaxLength(20);
            builder.HasIndex(a => a.Code).IsUnique();

            builder.Property(a => a.Naam).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Zone).IsRequired().HasMaxLength(50);

            builder.Property(a => a.MaxVoertuigLengteMeters).HasPrecision(4, 1);
            builder.Property(a => a.MaxCaravanLengteMeters).HasPrecision(4, 1);

            builder.Property(a => a.Ondergrond).HasMaxLength(30);
            builder.Property(a => a.Schaduw).HasMaxLength(20);
            builder.Property(a => a.WifiDekking).HasMaxLength(20);

            builder.HasMany(a => a.Voorzieningen)
                .WithMany(v => v.Accommodaties)
                .UsingEntity(j => j.ToTable("AccommodatieVoorzieningen"));
        }
    }
}
