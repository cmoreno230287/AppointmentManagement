using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AppointmentManagement.Domain.Entities;

namespace AppointmentManagement.Infrastructure.Persistence.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.Property(u => u.UserId)
                   .IsRequired();

            builder.Property(ur => ur.RoleId)
                   .IsRequired();
        }
    }
}
