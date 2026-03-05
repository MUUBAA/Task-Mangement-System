using TaskManagement.Service.TaskServices;

namespace TaskManagement.Utils
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();
            return services;
        }
    }
}
