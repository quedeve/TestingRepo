using domain.Models;
using main.DTOs;
using main.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService, IConfiguration config)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }


        
        private UserDto CreateUserObject(User user)
        {
            return new UserDto
            {
                Name = user.Name,
                Token = _tokenService.CreateToken(user),
                Email = user.Email,
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded && user.RowStatus)
            {
                return CreateUserObject(user);
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody]RegisterDto registerDto)
        {

            string ErrorMessage = "";
            var getUser = await _userManager.Users.Where(x => x.Email == registerDto.Email).FirstOrDefaultAsync();
            if (getUser != null && getUser.RowStatus == true)

            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }
            else if (getUser != null && getUser.RowStatus == false)
            {
           
                ModelState.AddModelError("email", "Failed to update reactive email");
                return ValidationProblem();
            }
            else
            {
                var user = new User
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
                    RowStatus = true
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    return CreateUserObject(user);
                }
                ErrorMessage = "Problems with registration";
                dynamic resultErrors = new
                {
                    ErrorMessage,
                    result.Errors
                };
                return Ok(resultErrors);
            }


        }


    }
}
