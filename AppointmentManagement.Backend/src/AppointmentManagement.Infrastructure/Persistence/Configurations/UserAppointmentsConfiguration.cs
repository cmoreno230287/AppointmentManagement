using AppointmentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagement.Infrastructure.Persistence.Configurations
{
    public class UserAppointmentsConfiguration : IEntityTypeConfiguration<UserAppointments>
    {
        public void Configure(EntityTypeBuilder<UserAppointments> builder)
        {
            builder.ToTable("UserAppointments");

            builder.HasKey(ua => ua.Id);

            builder.Property(ua => ua.Identifier)
                   .IsRequired()
                   .HasDefaultValueSql("NEWID()");

            builder.Property(ua => ua.UserId)
                   .IsRequired();

            builder.Property(ua => ua.AppointmentId)
                   .IsRequired();
        }
    }
}
