using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class PoolReservationConfiguration : IEntityTypeConfiguration<PoolReservation>
    {
        public void Configure(EntityTypeBuilder<PoolReservation> builder)
        {
            builder.ToTable("PoolReservations");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Date).IsRequired();
            builder.Property(r => r.Persons).IsRequired();

            builder.HasIndex(r => new { r.PoolTimeSlotId, r.Date });
        }
    }
}
