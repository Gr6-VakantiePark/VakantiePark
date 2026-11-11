using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class AfbeeldingConfiguration : IEntityTypeConfiguration<Afbeelding>
    {
        public void Configure(EntityTypeBuilder<Afbeelding> builder)
        {
            builder.ToTable("Afbeeldingen");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Bestandsnaam).IsRequired().HasMaxLength(200);
            builder.Property(a => a.ContentType).IsRequired().HasMaxLength(50);

            builder.HasIndex(a => new { a.AccommodatieTypeId, a.Volgorde });

            builder.HasOne(a => a.AccommodatieType)
                .WithMany(t => t.Afbeeldingen)
                .HasForeignKey(a => a.AccommodatieTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Bestand)
                .WithOne(b => b.Afbeelding)
                .HasForeignKey<AfbeeldingBestand>(b => b.AfbeeldingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class AfbeeldingBestandConfiguration : IEntityTypeConfiguration<AfbeeldingBestand>
    {
        public void Configure(EntityTypeBuilder<AfbeeldingBestand> builder)
        {
            builder.ToTable("AfbeeldingBestanden");
            builder.HasKey(b => b.AfbeeldingId);
            builder.Property(b => b.Data).IsRequired();
        }
    }
}
