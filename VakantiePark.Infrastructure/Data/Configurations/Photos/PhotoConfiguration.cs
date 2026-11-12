using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.ToTable("Photos");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FileName).IsRequired().HasMaxLength(200);
            builder.Property(a => a.ContentType).IsRequired().HasMaxLength(50);

            builder.HasIndex(a => new { a.AccommodationTypeId, a.SortOrder });

            builder.HasOne(a => a.AccommodationType)
                .WithMany(t => t.Photos)
                .HasForeignKey(a => a.AccommodationTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.File)
                .WithOne(b => b.Photo)
                .HasForeignKey<PhotoFile>(b => b.PhotoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class PhotoFileConfiguration : IEntityTypeConfiguration<PhotoFile>
    {
        public void Configure(EntityTypeBuilder<PhotoFile> builder)
        {
            builder.ToTable("PhotoFiles");
            builder.HasKey(b => b.PhotoId);
            builder.Property(b => b.Data).IsRequired();
        }
    }
}
