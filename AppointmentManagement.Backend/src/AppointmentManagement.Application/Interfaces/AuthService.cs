using AppointmentManagement.Application.Configuration;
using AppointmentManagement.Application.DTOs.Common;
using AppointmentManagement.Application.DTOs.Requests.Auth;
using AppointmentManagement.Application.DTOs.Responses.Auth;
using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Application.Interfaces.Services;
using AppointmentManagement.Application.Services.Common;
using AppointmentManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AppointmentManagement.Application.DTOs.Common.Extensions;
using System.Data;

namespace AppointmentManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly EncryptionService _encryptionService;
        private readonly AppSettings _appSettings;
        private readonly ILogger<AuthService> _logger;
        private static readonly ConcurrentDictionary<string, string> _refreshTokens = new();

        public AuthService(IUserRepository userRepository, EncryptionService encryptionService, AppSettings appSettings, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _appSettings = appSettings;
            _logger = logger;
        }

        public async Task<ApiResponse<LoginResponse>> AuthenticateUserAsync(LoginRequest request)
        {
            _logger.LogInformation($"Attempting to authenticate user: {request.Email}");

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !_encryptionService.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning($"Authentication failed for user {request.Email}: Invalid credentials.");
                return ApiResponse<LoginResponse>.ErrorResponse("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user.Id);

            _logger.LogInformation($"User {request.Email} authenticated successfully.");

            return ApiResponse<LoginResponse>.SuccessResponse(new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_appSettings.JwtSettings.ExpirationMinutes)
            });
        }

        public async Task<ApiResponse<LoginResponse>> RefreshTokenAsync(string token, string refreshToken)
        {
            _logger.LogInformation("Attempting to refresh access token.");
            var principal = GetPrincipalFromExpiredToken(token);
            if (principal == null || !_refreshTokens.TryGetValue(refreshToken, out var storedToken) || storedToken != token)
            {
                _logger.LogWarning("Refresh token validation failed: Invalid or expired token.");
                return ApiResponse<LoginResponse>.ErrorResponse("Invalid refresh token.");
            }

            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Refresh token validation failed: User not found.");
                return ApiResponse<LoginResponse>.ErrorResponse("User not found.");
            }

            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(user.Id);
            _refreshTokens.TryRemove(refreshToken, out _);

            _logger.LogInformation($"Access token refreshed successfully for user ID {userId}.");

            return ApiResponse<LoginResponse>.SuccessResponse(new LoginResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_appSettings.JwtSettings.ExpirationMinutes)
            });
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var userRoles = _userRepository.GetUserWithRolesAsync(user.Id).Result;
            var roles = userRoles?.Select(ur => ur.Role.Name).ToList() ?? new List<string>();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("username", user.Username)
            };

            // Add roles to claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _appSettings.JwtSettings.Issuer,
                audience: _appSettings.JwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_appSettings.JwtSettings.ExpirationMinutes),
                signingCredentials: credentials
            );

            _logger.LogInformation($"JWT token issued for user {user.Email} with roles: {string.Join(", ", roles)}.");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(int userId)
        {
            var refreshToken = Guid.NewGuid().ToString();
            _refreshTokens[refreshToken] = GenerateJwtToken(new User { Id = userId });
            return refreshToken;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _appSettings.JwtSettings.Issuer,
                    ValidAudience = _appSettings.JwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Secret)),
                    ValidateLifetime = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogWarning("JWT token validation failed: Invalid algorithm or tampered token.");
                    return null;
                }

                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"JWT token validation error: {ex.Message}");
                return null;
            }
        }
    }
}
