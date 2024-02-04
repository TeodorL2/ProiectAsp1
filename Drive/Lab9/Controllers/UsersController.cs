using Drive.Data.DTOs.UserDto;
using Drive.Data.Enums;
using Drive.Helpers.Attributes;
using Drive.Services.UserService;
using Drive2.Services.DirectoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Drive.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }



        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var response = _userService.Login(userDto);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        //[AllowAnonymous]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            var response = await _userService.Register(userDto, Data.Enums.Role.User);

            if (response == false)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin(UserDto userDto)
        {
            var response = await _userService.Register(userDto, Data.Enums.Role.Admin);

            if (response == false)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("check-auth-without-role")]
        public IActionResult GetText()
        {
            return Ok("Account is logged in");
        }


        [Authorize(Role.User)]
        [HttpGet("check-auth-user")]
        public IActionResult GetTextUser()
        {
            return Ok("User is logged in");
        }

        [Authorize(Role.Admin)]
        [HttpGet("check-auth-admin")]
        public IActionResult GetTextAdmin()
        {
            return Ok("Admin is logged in");
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpGet("check-auth-admin-and-user")]
        public IActionResult GetTextAdminUser()
        {
            return Ok("Account is user or admin");
        }

    }
}
