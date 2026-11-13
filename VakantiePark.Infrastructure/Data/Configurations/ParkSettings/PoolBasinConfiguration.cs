using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class PoolBasinConfiguration : IEntityTypeConfiguration<PoolBasin>
    {
        public void Configure(EntityTypeBuilder<PoolBasin> builder)
        {
            builder.ToTable("PoolBasins");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Size).IsRequired().HasMaxLength(100);
            builder.Property(p => p.SortOrder).IsRequired();
        }
    }
}
