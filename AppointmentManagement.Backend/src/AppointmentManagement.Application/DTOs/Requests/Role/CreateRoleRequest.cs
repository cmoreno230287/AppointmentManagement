﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.DTOs.Requests.Role
{
    public class CreateRoleRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
