using AppointmentManagement.Application.Interfaces.Persistence;
using AppointmentManagement.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            return services;
        }
    }
}
