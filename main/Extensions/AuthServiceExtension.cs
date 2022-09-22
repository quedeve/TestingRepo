using domain.Models;
using main.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using persistence;
using System.Text;

namespace main.Extensions
{
    public static class AuthServiceExtension
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;


            })
           .AddRoles<IdentityRole>()
           .AddDefaultTokenProviders()
           .AddEntityFrameworkStores<DataContext>()
           .AddSignInManager<SignInManager<User>>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.SaveToken = true;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = false,
                           ValidateAudience = false,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           IssuerSigningKey = key,
                           ClockSkew = TimeSpan.Zero
                       };
                   });


            services.AddScoped<TokenService>();

            return services;
        }
    }
}
