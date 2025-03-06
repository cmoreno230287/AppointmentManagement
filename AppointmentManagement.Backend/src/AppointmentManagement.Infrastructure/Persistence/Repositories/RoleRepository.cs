using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Domain.Entities;

namespace AppointmentManagement.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(AppointmentDbContext context) : base(context) { }
    }
}
