using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AppointmentManagement.Infrastructure.Persistence.Repositories
{
    public class UserAppointmentRepository : IUserAppointmentRepository
    {
        private readonly AppointmentDbContext _context;

        public UserAppointmentRepository(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAppointments>> GetAllByUserIdAsync(int userId)
        {
            return await _context.UserAppointments
                .Include(ua => ua.Appointment)
                .Include(ua => ua.User)
                .Where(ua => ua.UserId == userId)
                .OrderBy(ua => ua.Appointment.AppointmentDate)
                .ToListAsync();
        }

        public async Task<UserAppointments?> GetByIdAsync(int id)
        {
            return await _context.UserAppointments
                .Include(ua => ua.Appointment)
                .Include(ua => ua.User)
                .FirstOrDefaultAsync(ua => ua.Id == id);
        }

        public async Task<UserAppointments?> AddAsync(UserAppointments userAppointment)
        {
            await _context.UserAppointments.AddAsync(userAppointment);
            await _context.SaveChangesAsync();

            return new UserAppointments {
                Id = userAppointment.Id,
                UserId = userAppointment.UserId,
                AppointmentId = userAppointment.AppointmentId,
                
            };
        }

        public async Task UpdateAsync(UserAppointments userAppointment)
        {
            _context.UserAppointments.Update(userAppointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserAppointments userAppointment)
        {
            _context.UserAppointments.Remove(userAppointment);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<UserAppointments>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserAppointments>> FindAsync(Expression<Func<UserAppointments, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<UserAppointments?> IGenericRepository<UserAppointments>.UpdateAsync(UserAppointments entity)
        {
            throw new NotImplementedException();
        }
    }
}
