using AppointmentManagement.Domain.Entities;

namespace AppointmentManagement.Application.Interfaces.Persistence
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<UserRole?>> GetUserWithRolesAsync(int userId);
    }
}
