using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class OpeningHourConfiguration : IEntityTypeConfiguration<OpeningHour>
    {
        public void Configure(EntityTypeBuilder<OpeningHour> builder)
        {
            builder.ToTable("OpeningHours");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Facility).IsRequired().HasConversion<string>().HasMaxLength(20);
            builder.Property(o => o.Label).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Value).IsRequired().HasMaxLength(100);
            builder.Property(o => o.ShowOnHome).IsRequired();
            builder.Property(o => o.SortOrder).IsRequired();
        }
    }
}
