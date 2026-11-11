using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class AccommodatieTypeConfiguration : IEntityTypeConfiguration<AccommodatieType>
    {
        public void Configure(EntityTypeBuilder<AccommodatieType> builder)
        {
            builder.ToTable("AccommodatieTypes");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Naam).IsRequired().HasMaxLength(100);
            builder.HasIndex(t => t.Naam).IsUnique();

            builder.Property(t => t.Slug).IsRequired().HasMaxLength(100);
            builder.HasIndex(t => t.Slug).IsUnique();

            // Enum als tekst opslaan ("Huisje" / "Campingplaats") i.p.v. 1 / 2
            builder.Property(t => t.Categorie).IsRequired().HasConversion<string>().HasMaxLength(20);

            builder.Property(t => t.Omschrijving).HasMaxLength(2000);
            builder.Property(t => t.PrijsPerNacht).HasPrecision(10, 2);

            // List<string> wordt door EF Core als JSON-array in één kolom opgeslagen
            builder.Property(t => t.Afbeeldingen);

            builder.HasMany(t => t.Accommodaties)
                .WithOne(a => a.AccommodatieType)
                .HasForeignKey(a => a.AccommodatieTypeId)
                .OnDelete(DeleteBehavior.Restrict); // type niet verwijderen zolang er accommodaties aan hangen
        }
    }
}
