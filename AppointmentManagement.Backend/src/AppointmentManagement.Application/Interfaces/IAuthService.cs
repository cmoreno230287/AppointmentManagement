using AppointmentManagement.Application.DTOs.Common;
using AppointmentManagement.Application.DTOs.Requests.Auth;
using AppointmentManagement.Application.DTOs.Responses.Auth;

namespace AppointmentManagement.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>> AuthenticateUserAsync(LoginRequest request);
        Task<ApiResponse<LoginResponse>> RefreshTokenAsync(string token, string refreshToken);
    }
}
