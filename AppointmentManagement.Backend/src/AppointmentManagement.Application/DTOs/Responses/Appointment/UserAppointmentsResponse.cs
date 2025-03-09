using AppointmentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.DTOs.Responses.Appointment
{
    public class UserAppointmentsResponse
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int appointmentId { get; set; }
        public string notes { get; set; }
        public string status { get; set; }
        public string appointmentDate { get; set; }
        public string appointmentTime { get; set; }
    }
}
