using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

namespace TaskManagement.Utils
{
    public static class SwaggerProvider
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Task Management API",
                    Version = "v1",
                    Description =
                        "Simplified Task Management System.\n\n" +
                        "Login first to get a JWT:\n" +
                        "- POST /auth/login\n\n" +
                        "Then click **Authorize** and paste:\n" +
                        "Bearer {your_token}"
                });

                // Optional: stable operation ids
                options.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);

                // JWT Bearer setup for Swagger UI
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter: Bearer {your JWT token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });
        }
    }
}
