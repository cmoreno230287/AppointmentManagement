using AppointmentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.Interfaces.Persistence
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRole userRole);
        Task<UserRole?> GetUserRoleAsync(int userId, int roleId);
        Task<IEnumerable<UserRole?>> GetRolesByUserIdAsync(int userId);
        Task DeleteAsync(UserRole userRole);
    }
}
