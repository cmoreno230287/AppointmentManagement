using AppointmentManagement.Application.Interfaces.Services;
using AppointmentManagement.Application.DTOs.Requests.Appointment;
using AppointmentManagement.Application.DTOs.Responses.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AppointmentManagement.Application.Services;
using System.Security.Claims;

namespace AppointmentManagement.Api.Controllers
{
    [Authorize]
    [Route("api/v1/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var response = await _appointmentService.GetAllAppointmentsAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAppointmentsByUserId(int userId)
        {
            var response = await _appointmentService.GetAllAppointmentsAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetAppointmentsByCurrentUser()
        {
            int userId = int.Parse(HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _appointmentService.GetAppointmentsByUserId(userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var response = await _appointmentService.GetAppointmentByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            request.UserId = (string.IsNullOrEmpty(request.UserId)) ? HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value : request.UserId;
            var response = await _appointmentService.CreateAppointmentAsync(request);
            return response.Success ? CreatedAtAction(nameof(GetAppointmentById), new { id = response.Data?.Id }, response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentRequest request)
        {
            var response = await _appointmentService.UpdateAppointmentAsync(id, request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var response = await _appointmentService.DeleteAppointmentAsync(id);
            return response.Success ? NoContent() : NotFound(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var response = await _appointmentService.ApproveAppointmentAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var response = await _appointmentService.CancelAppointmentAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
