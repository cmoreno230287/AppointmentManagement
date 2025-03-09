using AppointmentManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.Interfaces.Persistence
{
    public interface IUserAppointmentRepository: IGenericRepository<UserAppointments>
    {
        Task<IEnumerable<UserAppointments>> GetAllByUserIdAsync(int userId);
    }
}
