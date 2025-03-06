using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManagement.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppointmentDbContext context) : base(context) { }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByUserIdAsync(int userId)
        {
            return await _context.Appointments
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
