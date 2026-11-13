using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Email).IsRequired().HasMaxLength(254);
            builder.HasIndex(e => e.Email).IsUnique();

            builder.Property(e => e.PasswordHash).IsRequired().HasMaxLength(100);
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.IsActive).HasDefaultValue(true);
            builder.Property(e => e.IsOwner).HasDefaultValue(false);
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasMany(e => e.Permissions)
                .WithMany(p => p.Employees)
                .UsingEntity(j => j.ToTable("EmployeePermissions"));
        }
    }

    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Code).IsRequired().HasMaxLength(100);
            builder.HasIndex(p => p.Code).IsUnique();

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        }
    }
}
