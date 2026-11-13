using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class PoolTimeSlotConfiguration : IEntityTypeConfiguration<PoolTimeSlot>
    {
        public void Configure(EntityTypeBuilder<PoolTimeSlot> builder)
        {
            builder.ToTable("PoolTimeSlots");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.DayOfWeek).IsRequired().HasConversion<string>().HasMaxLength(10);
            builder.Property(t => t.StartTime).IsRequired();
            builder.Property(t => t.EndTime).IsRequired();
            builder.Property(t => t.Capacity).IsRequired();

            builder.HasIndex(t => new { t.DayOfWeek, t.StartTime }).IsUnique();

            builder.HasMany(t => t.Reservations)
                .WithOne(r => r.PoolTimeSlot)
                .HasForeignKey(r => r.PoolTimeSlotId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
