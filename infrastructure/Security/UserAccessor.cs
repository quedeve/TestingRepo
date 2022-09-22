using application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Security
{
    public class UserAccessor :IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetEmail()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }
    }
}
