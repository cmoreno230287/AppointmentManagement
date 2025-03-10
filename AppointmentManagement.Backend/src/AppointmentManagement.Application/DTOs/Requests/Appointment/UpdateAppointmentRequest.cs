using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.DTOs.Requests.Appointment
{
    public class UpdateAppointmentRequest
    {
        [Required]
        public string Note { get; set; } = string.Empty;

        [Required]
        public DateTime AppointmentDate { get; set; }
    }
}
