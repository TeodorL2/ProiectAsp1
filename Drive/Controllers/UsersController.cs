using Drive.Data.DTOs;
using Drive.Data.Enums;
using Drive.Data.Exceptions;
using Drive.Helpers.Attributes;
using Drive.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Drive.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Login(UserLoginRequestDto userLoginDto)
        {
            var response = await _userService.Login(userLoginDto);
            Console.WriteLine("Raspuns la login: " + response);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(UserRegisterRequestDto userRegisterDto)
        {
            try
            {
                var response = await _userService.Register(userRegisterDto, Role.User);

                return Ok(response);
            }
            catch (BadUser ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ErrorDuringSavingContext ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin(UserRegisterRequestDto userRegisterDto)
        {
            try
            {
                var response = await _userService.Register(userRegisterDto, Role.Admin);

                return Ok(response);
            }
            catch (BadUser ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ErrorDuringSavingContext ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
