using AppointmentManagement.Domain.Entities;

namespace AppointmentManagement.Application.Interfaces.Persistence
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(int userId);
    }
}
