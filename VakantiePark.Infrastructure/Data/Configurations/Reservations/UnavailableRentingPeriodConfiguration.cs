using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class UnavailableRentingPeriodConfiguration : IEntityTypeConfiguration<UnrentablePeriod>
    {
        public void Configure(EntityTypeBuilder<UnrentablePeriod> builder)
        {
            builder.ToTable("UnrentablePeriods");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.StartDate).IsRequired();
            builder.Property(u => u.EndDate).IsRequired();
            builder.Property(u => u.Reason).IsRequired();
            builder.Property(u => u.AccommodationId).IsRequired();

            builder.HasOne(u => u.Accommodation)
                .WithMany()
                .HasForeignKey(u => u.AccommodationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
