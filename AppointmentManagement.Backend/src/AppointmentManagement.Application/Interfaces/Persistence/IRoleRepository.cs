using AppointmentManagement.Domain.Entities;

namespace AppointmentManagement.Application.Interfaces.Persistence
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
    }
}
