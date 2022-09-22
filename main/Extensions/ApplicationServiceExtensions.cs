using application.Core;
using application.Employees;
using application.Interfaces;
using infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using persistence;

namespace main.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DevConnection"));
                //opt.UseMySql(config.GetConnectionString("DevConnection"), ServerVersion.AutoDetect(config.GetConnectionString("DevConnection")));
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader();
                });
            });

            //MediatR Add Assembly

            
            services.AddMediatR(typeof(EmployeeList.Handler).Assembly);
            //Clear messy assembly

            //End MediatR Add Assembly

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddScoped<IUserAccessor, UserAccessor>();

            return services;
        }
    }
}
