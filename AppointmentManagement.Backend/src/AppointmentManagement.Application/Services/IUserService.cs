using AppointmentManagement.Application.DTOs.Common;
using AppointmentManagement.Application.DTOs.Requests.User;
using AppointmentManagement.Application.DTOs.Responses.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<UserResponse>>> GetAllUsersAsync();
        Task<ApiResponse<UserResponse?>> GetUserByIdAsync(int id);
        Task<ApiResponse<UserResponse?>> GetUserByEmailAsync(string email);
        Task<ApiResponse<UserResponse>> CreateUserAsync(CreateUserRequest request);
        Task<ApiResponse<UserResponse?>> UpdateUserAsync(int id, UpdateUserRequest request);
        Task<ApiResponse<bool>> DeleteUserAsync(int id);
        Task<ApiResponse<bool>> VerifyUserPasswordAsync(string email, string password);
    }
}
