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
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Identifier)
                   .IsRequired()
                   .HasDefaultValueSql("NEWID()");

            builder.Property(a => a.Notes)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(a => a.Status)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
