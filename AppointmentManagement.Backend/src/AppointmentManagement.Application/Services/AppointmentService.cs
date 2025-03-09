using AppointmentManagement.Application.DTOs.Common;
using AppointmentManagement.Application.DTOs.Requests.Appointment;
using AppointmentManagement.Application.DTOs.Responses.Appointment;
using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Application.Interfaces.Services;
using AppointmentManagement.Domain.Entities;
using AppointmentManagement.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserAppointmentRepository _userAppointmentRepository;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(IAppointmentRepository appointmentRepository, IUserAppointmentRepository userAppointmentRepository, ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _userAppointmentRepository = userAppointmentRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<AppointmentResponse>>> GetAllAppointmentsAsync()
        {
            _logger.LogInformation("Retrieving all appointments...");
            var appointments = await _appointmentRepository.GetAllAsync();
            var responseList = appointments.Select(a => new AppointmentResponse
            {
                Id = a.Id,
                Notes = a.Notes,
                AppointmentDate = a.AppointmentDate,
                AppointmentTime = a.AppointmentTime,
                Status = a.Status.ToString()                
            }).ToList();

            return ApiResponse<IEnumerable<AppointmentResponse>>.SuccessResponse(responseList);
        }

        public async Task<ApiResponse<AppointmentResponse>> GetAppointmentByIdAsync(int id)
        {
            _logger.LogInformation($"Retrieving appointment with ID: {id}");
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                _logger.LogWarning($"Appointment with ID {id} not found.");
                return ApiResponse<AppointmentResponse>.ErrorResponse("Appointment not found.");
            }

            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                Notes = appointment.Notes,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                Status = appointment.Status.ToString()
            };

            return ApiResponse<AppointmentResponse>.SuccessResponse(response);
        }

        public async Task<ApiResponse<AppointmentResponse>> CreateAppointmentAsync(CreateAppointmentRequest request)
        {
            _logger.LogInformation("Creating new appointment...");

            var appointment = new Appointment
            {
                Notes = request.Note,
                AppointmentDate = request.AppointmentDate,
                AppointmentTime = TimeSpan.Parse(request.AppointmentDate.ToString("HH:mm")),
                Status = AppointmentStatus.Pending                
            };

            var appointmentSaved = await _appointmentRepository.AddAsync(appointment);
            if (appointmentSaved != null)
            {
                var userAppointment = new UserAppointments
                {
                    AppointmentId = appointment.Id,
                    UserId = int.Parse(request.UserId)
                };

                await _userAppointmentRepository.AddAsync(userAppointment);
                _logger.LogInformation($"New appointment assigned to user id ${userAppointment.UserId} successfully");
            }
            else {
                return ApiResponse<AppointmentResponse>.ErrorResponse("Error to save the appointment");
            }

            _logger.LogInformation("New appointment created successfully");

            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                Notes = appointment.Notes,
                Status = appointment.Status.ToString(),
                AppointmentDate = appointment.AppointmentDate
            };

            return ApiResponse<AppointmentResponse>.SuccessResponse(response);
        }
        public async Task<ApiResponse<AppointmentResponse>> UpdateAppointmentAsync(int id, UpdateAppointmentRequest request)
        {
            _logger.LogInformation($"Updating appointment with ID: {id}");
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                _logger.LogWarning($"Appointment with ID {id} not found.");
                return ApiResponse<AppointmentResponse>.ErrorResponse("Appointment not found.");
            }

            if(appointment.Status != AppointmentStatus.Pending)
            {
                _logger.LogWarning($"The appointment only can be updated if it is Pending.");
                return ApiResponse<AppointmentResponse>.ErrorResponse($"The appointment only can be updated if it is Pending.");
            }

            appointment.Notes = request.Note;
            appointment.AppointmentDate = request.AppointmentDate;
            appointment.Status = Enum.Parse<AppointmentStatus>(request.Status);

            await _appointmentRepository.UpdateAsync(appointment);

            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                Notes = appointment.Notes,
                Status = appointment.Status.ToString(),
                AppointmentDate = appointment.AppointmentDate
            };

            return ApiResponse<AppointmentResponse>.SuccessResponse(response);
        }

        public async Task<ApiResponse<bool>> ApproveAppointmentAsync(int id)
        {
            _logger.LogInformation($"Approving appointment with ID: {id}");
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.Status == AppointmentStatus.Canceled)
            {
                _logger.LogWarning("Cannot approve a non-existent or canceled appointment.");
                return ApiResponse<bool>.ErrorResponse("Cannot approve a non-existent or canceled appointment.");
            }

            appointment.Status = AppointmentStatus.Approved;
            await _appointmentRepository.UpdateAsync(appointment);
            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<bool>> CancelAppointmentAsync(int id)
        {
            _logger.LogInformation($"Canceling appointment with ID: {id}");
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                _logger.LogWarning("Cannot cancel a non-existent appointment.");
                return ApiResponse<bool>.ErrorResponse("Cannot cancel a non-existent appointment.");
            }

            appointment.Status = AppointmentStatus.Canceled;
            await _appointmentRepository.UpdateAsync(appointment);
            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<bool>> DeleteAppointmentAsync(int id)
        {
            _logger.LogInformation($"Deleting appointment with ID: {id}");
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.Status != AppointmentStatus.Canceled)
            {
                _logger.LogWarning("Cannot delete an appointment unless it is first canceled.");
                return ApiResponse<bool>.ErrorResponse("Cannot delete an appointment unless it is first canceled.");
            }

            await _appointmentRepository.DeleteAsync(appointment);
            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<IEnumerable<UserAppointmentsResponse>>> GetAppointmentsByUserId(int userId)
        {
            _logger.LogInformation("Retrieving all appointments by user...");
            var appointmentsbyuser = await _userAppointmentRepository.GetAllByUserIdAsync(userId);

            if (appointmentsbyuser == null)
            {
                _logger.LogWarning($"Any appointment was found for user {userId}.");
                return ApiResponse<IEnumerable<UserAppointmentsResponse>>.ErrorResponse($"Any appointment was found for user {userId}.");
            }

            var responseList = appointmentsbyuser.Select(a => new UserAppointmentsResponse
            {
                id = a.Id,
                notes = a.Appointment.Notes,
                appointmentId = a.Appointment.Id,
                userId = a.User.Id,
                status = a.Appointment.Status.ToString(),
                appointmentDate = a.Appointment.AppointmentDate.ToShortDateString(),
                appointmentTime = a.Appointment.AppointmentTime.ToString()
                
            }).ToList();

            return ApiResponse<IEnumerable<UserAppointmentsResponse>>.SuccessResponse(responseList);
        }
    }
}
