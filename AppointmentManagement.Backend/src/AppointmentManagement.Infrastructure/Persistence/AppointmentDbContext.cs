using AppointmentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace AppointmentManagement.Infrastructure.Persistence
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentDbContext).Assembly);

            // Use static GUIDs instead of `Guid.NewGuid()`
            var userRoleId = new Guid("ad3d695e-726a-4985-970c-b8cacf61b730");
            var managerRoleId = new Guid("5984a644-b3ef-4a8c-84f5-e3bd4d5180d5");
            var adminUserId = new Guid("ecf36fea-59a0-42b6-9005-e4c09e421444");
            var createdAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Identifier = userRoleId, Name = "User", Description = "Regular User" },
                new Role { Id = 2, Identifier = managerRoleId, Name = "Manager", Description = "Appointment Manager" }
            );

            // Seed Users (Default Admin)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Identifier = adminUserId,
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = "Admin@123", //BCrypt.Net.BCrypt.HashPassword("Admin@123"), // Hash the password
                    IsActive = true,
                    CreatedAt = createdAt,
                    UpdatedAt = null,
                    UpdatedBy = new Guid("ecf36fea-59a0-42b6-9005-e4c09e421444"),
                    CreatedBy = new Guid("ecf36fea-59a0-42b6-9005-e4c09e421444")
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
