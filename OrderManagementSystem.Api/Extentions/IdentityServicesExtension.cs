using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Services;
using OrderManagementSystem.Repsitory.Identity;
using OrderManagementSystem.Services.AuthService;
using System.Text;

namespace OrderManagementSystem.Api.Extentions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration _configuration)
        {

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbConext>();

            services.AddScoped<IAuthService, AuthService  >();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))

                }
                );
            return services;
        }
    }
}
