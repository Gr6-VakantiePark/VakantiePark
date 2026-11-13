using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;
using VakantiePark.Shared.Common;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.ToTable("Guests");
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name).IsRequired().HasMaxLength(FieldLimits.GuestNameMaxLength);
            builder.Property(g => g.Email).IsRequired().HasMaxLength(FieldLimits.EmailMaxLength);
            builder.Property(g => g.PhoneNumber).HasMaxLength(FieldLimits.PhoneNumberMaxLength);
        }
    }
}
