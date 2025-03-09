using AppointmentManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Domain.Entities
{
    public class Appointment : EntityBase
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentTime { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [MaxLength(300)]
        public string? Notes { get; set; }
    }
}
