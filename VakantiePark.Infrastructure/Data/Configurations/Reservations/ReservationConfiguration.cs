using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
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

            builder.HasOne(r => r.Accommodation)
                .WithMany()
                .HasForeignKey(r => r.AccommodationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
