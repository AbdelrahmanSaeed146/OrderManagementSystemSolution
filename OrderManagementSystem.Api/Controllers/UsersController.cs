using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Entities.Dtos;
using OrderManagementSystem.Core.Services;
using OrderManagementSystem.Services.AuthService;

namespace OrderManagementSystem.Api.Controllers
{

    public class UsersController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        
        private readonly IAuthService _authService;


        public UsersController(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, IAuthService authService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }

            [HttpPost("register")]
           
    public async Task<ActionResult<UserDto>> Register(RegisterDto model)
    {
            if (await CheckEmailExists(model.Email))
            {
                return BadRequest(new { Message = "Email is already in use" });
            }

            var user = new User
            {
                UserName = model.Email.Split('@')[0],
                Email = model.Email,
                DisplayName = model.UserName,
               
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            if (!string.IsNullOrEmpty(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            var returnedUser = new UserDto
            {
                Name = user.DisplayName,
               Email  = user.Email,
               Token = await _authService.GetTokenAsync(user, _userManager)
            };

            return Ok(returnedUser);
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            

            var User = await _userManager.FindByEmailAsync(model.Email);

            if (User is null) return Unauthorized();

            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);

            if (!Result.Succeeded) return Unauthorized();
            var ReturnedUser = new UserDto()
            {
                Name = User.DisplayName,
                Email = User.Email,
                Token = await _authService.GetTokenAsync(User, _userManager)
            };
            return Ok(ReturnedUser);

            
        }







        private async Task<bool> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}

