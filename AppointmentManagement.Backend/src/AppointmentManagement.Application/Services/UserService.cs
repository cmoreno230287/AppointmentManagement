using AppointmentManagement.Application.DTOs.Common;
using AppointmentManagement.Application.DTOs.Requests.User;
using AppointmentManagement.Application.DTOs.Responses.User;
using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Application.Interfaces.Services;
using AppointmentManagement.Application.Services.Common;
using AppointmentManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly EncryptionService _encryptionService;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, EncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _encryptionService = encryptionService;
        }

        public async Task<ApiResponse<IEnumerable<UserResponse>>> GetAllUsersAsync()
        {
            _logger.LogInformation("Retrieving all users...");
            var users = await _userRepository.GetAllAsync();
            var userResponses = users.Select(user => new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive
            }).ToList();

            return ApiResponse<IEnumerable<UserResponse>>.SuccessResponse(userResponses);
        }

        public async Task<ApiResponse<UserResponse?>> GetUserByIdAsync(int id)
        {
            _logger.LogInformation($"Retrieving user with ID: {id}");
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return ApiResponse<UserResponse?>.ErrorResponse("User not found.");
            }

            var response = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive
            };

            return ApiResponse<UserResponse?>.SuccessResponse(response);
        }

        public async Task<ApiResponse<UserResponse?>> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation($"Retrieving user with Email: {email}");
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"User with Email {email} not found.");
                return ApiResponse<UserResponse?>.ErrorResponse("User not found.");
            }

            var response = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive
            };

            return ApiResponse<UserResponse?>.SuccessResponse(response);
        }

        public async Task<ApiResponse<UserResponse>> CreateUserAsync(CreateUserRequest request)
        {
            _logger.LogInformation($"Creating new user: {request.Email}");

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning($"User with email {request.Email} already exists.");
                return ApiResponse<UserResponse>.ErrorResponse("User already exists.");
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _encryptionService.HashPassword(request.Password),
                IsActive = true
            };

            await _userRepository.AddAsync(user);
            _logger.LogInformation($"User {user.Email} created successfully.");

            var response = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive
            };

            return ApiResponse<UserResponse>.SuccessResponse(response);
        }

        public async Task<ApiResponse<UserResponse?>> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            _logger.LogInformation($"Updating user with ID: {id}");
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return ApiResponse<UserResponse?>.ErrorResponse("User not found.");
            }

            user.Username = request.Username;
            user.Email = request.Email;
            user.IsActive = request.IsActive;

            await _userRepository.UpdateAsync(user);
            _logger.LogInformation($"User {id} updated successfully.");

            var response = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive
            };

            return ApiResponse<UserResponse?>.SuccessResponse(response);
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
        {
            _logger.LogInformation($"Deleting user with ID: {id}");
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return ApiResponse<bool>.ErrorResponse("User not found.");
            }

            await _userRepository.DeleteAsync(user);
            _logger.LogInformation($"User {id} deleted successfully.");
            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<bool>> VerifyUserPasswordAsync(string email, string password)
        {
            _logger.LogInformation($"Verifying password for user: {email}");
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"User with email {email} not found.");
                return ApiResponse<bool>.ErrorResponse("User not found.");
            }

            bool isPasswordValid = _encryptionService.VerifyPassword(password, user.PasswordHash);
            return ApiResponse<bool>.SuccessResponse(isPasswordValid);
        }
    }
}
