using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManagement.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppointmentDbContext _context;
        public UserRepository(AppointmentDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<UserRole?>> GetUserWithRolesAsync(int userId)
        {
            return await _context.UsersRoles
                .Where(u => u.UserId == userId)
                .Select(s => new UserRole
                {
                    Id = s.Id,
                    RoleId = s.RoleId,
                    UserId = userId,
                    Role = new Role
                    {
                        Name = s.Role.Name
                    },
                    User = new User
                    {
                        Username = s.User.Username,
                    }
                }).ToListAsync();
        }
    }
}
