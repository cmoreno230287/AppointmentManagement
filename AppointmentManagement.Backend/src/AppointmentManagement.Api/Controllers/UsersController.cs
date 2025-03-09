using AppointmentManagement.Application.DTOs.Requests.User;
using AppointmentManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppointmentManagement.Api.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers() => Ok(await _userService.GetAllUsersAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUserInformationByCurrentUserId()
        {
            int userId = int.Parse(HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            return user != null ? Ok(user) : NotFound();
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Data.Id }, user);
        }
    }
}
