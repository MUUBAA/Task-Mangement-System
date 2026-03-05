using Microsoft.EntityFrameworkCore;
using System;
using TaskManagement.Data.Repositories;

namespace TaskManagement.Utils
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connString))
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connString));

            // Repository DI
            services.AddScoped<ITaskRepository, TaskRepository>();

            return services;
        }
    }
}
