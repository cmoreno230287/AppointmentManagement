using AppointmentManagement.Domain.Entities;

namespace AppointmentManagement.Application.Interfaces.Persistence
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<UserAppointments>> GetAppointmentsByUserIdAsync(int userId);
    }
}
