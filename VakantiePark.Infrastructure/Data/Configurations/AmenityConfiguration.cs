using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class VoorzieningConfiguration : IEntityTypeConfiguration<Voorziening>
    {
        public void Configure(EntityTypeBuilder<Voorziening> builder)
        {
            builder.ToTable("Voorzieningen");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(v => v.Code).IsUnique();

            builder.Property(v => v.Naam).IsRequired().HasMaxLength(100);
        }
    }
}
