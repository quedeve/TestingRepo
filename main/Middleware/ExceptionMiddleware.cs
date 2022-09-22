
using application.Core;
using Serilog;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace main.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _config;


        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env, IConfiguration config)
        {
            _next = next;
            _logger = logger;
            _env = env;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string body = "";
            var req = context.Request;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                
                var response = _env.IsDevelopment() ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new AppException(context.Response.StatusCode, "Sever Error");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
