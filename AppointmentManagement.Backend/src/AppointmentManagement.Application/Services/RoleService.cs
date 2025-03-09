using AppointmentManagement.Application.DTOs.Common;
using AppointmentManagement.Application.DTOs.Requests.Role;
using AppointmentManagement.Application.DTOs.Responses.Role;
using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Application.Interfaces.Services;
using AppointmentManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RoleService> _logger;
        private readonly IUserRoleRepository _userRoleRepository;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository, IUserRoleRepository userRoleRepository, ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<RoleResponse>>> GetAllRolesAsync()
        {
            _logger.LogInformation("Retrieving all roles...");
            var roles = await _roleRepository.GetAllAsync();
            var roleResponses = roles.Select(role => new RoleResponse
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();

            return ApiResponse<IEnumerable<RoleResponse>>.SuccessResponse(roleResponses);
        }
        public async Task<ApiResponse<IEnumerable<RoleResponse>>> GetRolesByUserAsync(int userId)
        {
            _logger.LogInformation("Retrieving all roles...");
            var roles = await _userRoleRepository.GetRolesByUserIdAsync(userId);
            var roleResponses = roles.Select(role => new RoleResponse
            {
                Id = role.Id,
                Name = role.Role.Name
            }).ToList();

            return ApiResponse<IEnumerable<RoleResponse>>.SuccessResponse(roleResponses);
        }

        public async Task<ApiResponse<RoleResponse?>> GetRoleByIdAsync(int id)
        {
            _logger.LogInformation($"Retrieving role with ID: {id}");
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                _logger.LogWarning($"Role with ID {id} not found.");
                return ApiResponse<RoleResponse?>.ErrorResponse("Role not found.");
            }

            var response = new RoleResponse
            {
                Id = role.Id,
                Name = role.Name
            };

            return ApiResponse<RoleResponse?>.SuccessResponse(response);
        }

        public async Task<ApiResponse<RoleResponse>> CreateRoleAsync(CreateRoleRequest request)
        {
            _logger.LogInformation($"Creating new role: {request.Name}");

            var role = new Role
            {
                Name = request.Name
            };

            await _roleRepository.AddAsync(role);
            _logger.LogInformation($"Role {role.Name} created successfully.");

            var response = new RoleResponse
            {
                Id = role.Id,
                Name = role.Name
            };

            return ApiResponse<RoleResponse>.SuccessResponse(response);
        }

        public async Task<ApiResponse<RoleResponse?>> UpdateRoleAsync(int id, UpdateRoleRequest request)
        {
            _logger.LogInformation($"Updating role with ID: {id}");
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                _logger.LogWarning($"Role with ID {id} not found.");
                return ApiResponse<RoleResponse?>.ErrorResponse("Role not found.");
            }

            role.Name = request.Name;

            await _roleRepository.UpdateAsync(role);
            _logger.LogInformation($"Role {id} updated successfully.");

            var response = new RoleResponse
            {
                Id = role.Id,
                Name = role.Name
            };

            return ApiResponse<RoleResponse?>.SuccessResponse(response);
        }

        public async Task<ApiResponse<bool>> DeleteRoleAsync(int id)
        {
            _logger.LogInformation($"Deleting role with ID: {id}");
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                _logger.LogWarning($"Role with ID {id} not found.");
                return ApiResponse<bool>.ErrorResponse("Role not found.");
            }

            await _roleRepository.DeleteAsync(role);
            _logger.LogInformation($"Role {id} deleted successfully.");
            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<bool>> AssignRoleToUserAsync(int userId, int roleId)
        {
            _logger.LogInformation($"Assigning role {roleId} to user {userId}...");
            var user = await _userRepository.GetByIdAsync(userId);
            var role = await _roleRepository.GetByIdAsync(roleId);

            if (user == null || role == null)
            {
                _logger.LogWarning("User or Role not found.");
                return ApiResponse<bool>.ErrorResponse("User or Role not found.");
            }

            var userRole = new UserRole { UserId = userId, RoleId = roleId };
            await _userRoleRepository.AddAsync(userRole);

            _logger.LogInformation($"Role {roleId} assigned to user {userId} successfully.");
            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<bool>> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            _logger.LogInformation($"Removing role {roleId} from user {userId}...");
            var userRole = await _userRoleRepository.GetUserRoleAsync(userId, roleId);
            if (userRole == null)
            {
                _logger.LogWarning("UserRole not found.");
                return ApiResponse<bool>.ErrorResponse("UserRole not found.");
            }

            await _userRoleRepository.DeleteAsync(userRole);
            _logger.LogInformation($"Role {roleId} removed from user {userId} successfully.");
            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}
