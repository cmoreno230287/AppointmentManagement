using AppointmentManagement.Application.DTOs.Common;
using AppointmentManagement.Application.DTOs.Requests.Appointment;
using AppointmentManagement.Application.DTOs.Responses.Appointment;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.Services
{
    public interface IAppointmentService
    {
        Task<ApiResponse<IEnumerable<AppointmentResponse>>> GetAllAppointmentsAsync();
        Task<ApiResponse<IEnumerable<UserAppointmentsResponse>>> GetAppointmentsByUserId(int userId);
        Task<ApiResponse<AppointmentResponse?>> GetAppointmentByIdAsync(int id);
        Task<ApiResponse<AppointmentResponse>> CreateAppointmentAsync(CreateAppointmentRequest request);
        Task<ApiResponse<AppointmentResponse>> UpdateAppointmentAsync(int id, UpdateAppointmentRequest request);
        Task<ApiResponse<bool>> ApproveAppointmentAsync(int id);
        Task<ApiResponse<bool>> CancelAppointmentAsync(int id);
        Task<ApiResponse<bool>> DeleteAppointmentAsync(int id);
    }
}
