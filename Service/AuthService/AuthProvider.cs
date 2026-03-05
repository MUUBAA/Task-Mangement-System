using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TaskManagement.Service.AuthService
{
    public static class AuthProvider
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var secret = configuration["Jwt:Secret"];
            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException("Jwt:Secret is missing in configuration.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; // OK for local/dev assessment
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,

                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(); // enables [Authorize] and [Authorize(Roles="Admin")]
        }
    }

}
