using AppointmentManagement.Application.DTOs.Common;
using AppointmentManagement.Application.DTOs.Responses.Role;
using AppointmentManagement.Application.DTOs.Requests.Role;

namespace AppointmentManagement.Application.Services
{
    public interface IRoleService
    {
        Task<ApiResponse<IEnumerable<RoleResponse>>> GetAllRolesAsync();
        Task<ApiResponse<IEnumerable<RoleResponse>>> GetRolesByUserAsync(int userId);
        Task<ApiResponse<RoleResponse?>> GetRoleByIdAsync(int id);
        Task<ApiResponse<RoleResponse>> CreateRoleAsync(CreateRoleRequest request);
        Task<ApiResponse<RoleResponse?>> UpdateRoleAsync(int id, UpdateRoleRequest request);
        Task<ApiResponse<bool>> DeleteRoleAsync(int id);
        Task<ApiResponse<bool>> AssignRoleToUserAsync(int userId, int roleId);
        Task<ApiResponse<bool>> RemoveRoleFromUserAsync(int userId, int roleId);
    }
}
