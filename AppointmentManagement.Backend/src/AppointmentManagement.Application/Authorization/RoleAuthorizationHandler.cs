using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Domain.Entities;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace AppointmentManagement.Application.Authorization
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RoleAuthorizationHandler> _logger;

        public RoleAuthorizationHandler(IUserRoleRepository userRoleRepository, IHttpContextAccessor httpContextAccessor, ILogger<RoleAuthorizationHandler> logger)
        {
            _userRoleRepository = userRoleRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Authorization failed: No valid user ID found in claims.");
                context.Fail();
                return;
            }

            var userRoles = await _userRoleRepository.GetRolesByUserIdAsync(userId);
            if (userRoles.Any(ur => ur.Role.Name == requirement.Role))
            {
                _logger.LogInformation($"Authorization successful: User {userId} has role {requirement.Role}.");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogWarning($"Authorization failed: User {userId} does not have the required role {requirement.Role}.");
                context.Fail();
            }
        }
    }

    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }

        public RoleRequirement(string role)
        {
            Role = role;
        }
    }
}
