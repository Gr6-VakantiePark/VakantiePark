using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.StartDate).IsRequired();
            builder.Property(r => r.EndDate).IsRequired();
            builder.Property(r => r.NumberOfGuests).IsRequired();
            builder.Property(r => r.Status).IsRequired();
            builder.Property(r => r.AccommodationId).IsRequired();

            builder.Property(r => r.BookingSequence).IsRequired();
            builder.HasIndex(r => r.BookingSequence).IsUnique();

            builder.Property(r => r.BookingNumber).IsRequired().HasMaxLength(20);
            builder.HasIndex(r => r.BookingNumber).IsUnique();

            builder.Property(r => r.TotalPrice).HasPrecision(10, 2);
            builder.Property(r => r.CreatedAt).IsRequired();

            builder.HasOne(r => r.Accommodation)
                .WithMany()
                .HasForeignKey(r => r.AccommodationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Guest)
                .WithMany(g => g.Reservations)
                .HasForeignKey(r => r.GuestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
