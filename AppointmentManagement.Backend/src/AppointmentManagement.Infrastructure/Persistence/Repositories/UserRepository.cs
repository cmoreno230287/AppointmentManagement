﻿using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManagement.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppointmentDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
