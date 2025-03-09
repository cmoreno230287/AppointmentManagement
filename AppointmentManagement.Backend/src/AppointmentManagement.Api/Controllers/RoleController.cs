using AppointmentManagement.Application.DTOs.Requests.Role;
using AppointmentManagement.Application.Interfaces.Services;
using AppointmentManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppointmentManagement.Api.Controllers
{
    [Route("api/v1/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await _roleService.GetAllRolesAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var response = await _roleService.GetRoleByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var response = await _roleService.CreateRoleAsync(request);
            return response.Success ? CreatedAtAction(nameof(GetRoleById), new { id = response.Data?.Id }, response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleRequest request)
        {
            var response = await _roleService.UpdateRoleAsync(id, request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var response = await _roleService.DeleteRoleAsync(id);
            return response.Success ? NoContent() : NotFound(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoleToUser(int userId, int roleId)
        {
            var response = await _roleService.AssignRoleToUserAsync(userId, roleId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRoleFromUser(int userId, int roleId)
        {
            var response = await _roleService.RemoveRoleFromUserAsync(userId, roleId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
