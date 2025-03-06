using AppointmentManagement.Application.DTOs.Requests.Auth;
using AppointmentManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentManagement.Api.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticate a user and return access & refresh tokens.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.AuthenticateUserAsync(request);
            return response.Success ? Ok(response) : Unauthorized(response);
        }

        /// <summary>
        /// Refresh an expired access token using a valid refresh token.
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);
            return response.Success ? Ok(response) : Unauthorized(response);
        }
    }
}
