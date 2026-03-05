using Microsoft.EntityFrameworkCore;
using TaskManagement.Service.AuthService;
using TaskManagement.Utils;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
SwaggerProvider.Configure(builder.Services); 

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Auth (JWT)
AuthProvider.Configure(builder.Services, builder.Configuration);
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();

}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();