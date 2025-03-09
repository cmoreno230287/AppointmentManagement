using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AppointmentManagement.Infrastructure.Persistence.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppointmentDbContext _context;

        public UserRoleRepository(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserRole userRole)
        {
            await _context.UsersRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task<UserRole?> GetUserRoleAsync(int userId, int roleId)
        {
            return await _context.UsersRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }

        public async Task<IEnumerable<UserRole?>> GetRolesByUserIdAsync(int userId)
        {
            return await _context.UsersRoles.Where(ur => ur.UserId == userId).Select(ur => new UserRole
            {
                Id = ur.Id,
                RoleId = ur.RoleId,
                UserId = ur.UserId,
                Role = new Role
                {
                    Name = ur.Role.Name
                },
                User = new User
                {
                    Username = ur.User.Username,
                    Email = ur.User.Email
                }
            }).ToListAsync();
        }

        public async Task DeleteAsync(UserRole userRole)
        {
            _context.UsersRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
