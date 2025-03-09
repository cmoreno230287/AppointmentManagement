using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppointmentManagement.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDbContext _context;
        private readonly IUserAppointmentRepository _userAppointmentRepository;

        public AppointmentRepository(AppointmentDbContext context, IUserAppointmentRepository userAppointmentRepository)
        {
            _context = context;
            _userAppointmentRepository = userAppointmentRepository;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<Appointment?> AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment?> UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserAppointments>> GetAppointmentsByUserIdAsync(int userId)
        {
            return await _userAppointmentRepository.GetAllByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Appointment>> FindAsync(Expression<Func<Appointment, bool>> predicate)
        {
            return await _context.Appointments
                .Where(predicate)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}
