using AppointmentManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Domain.Entities
{
    public class Appointment : EntityBase
    {
        public int UserId { get; set; }
        public int? ManagerId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentTime { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
