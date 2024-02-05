using Drive.Data.DTOs.UserDto;
using Drive.Data.Enums;
using Drive.Data.Models;
using Drive.Helpers.Attributes;
using Drive.Services.UserService;
using Drive2.Services.DirectoryService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Drive2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DirectoryController : ControllerBase
    {
        private readonly IDirectoryService _directoryService;

        public DirectoryController(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
            Console.WriteLine("DirectoryController [controller] was built");
        }

        // [EnableCors]
        [Authorize(Role.Admin, Role.User)]
        [HttpGet("{*path}")]
        public IActionResult GetEntries(string path)
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;
            Guid? userId;
            if(authorizedUser == null) { userId = null; }
            else
            {  userId = authorizedUser.Id; }
            var response = _directoryService.GetByPath(path, userId);
            Console.WriteLine("DirectoryController");
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpPost("{*path}")]
        public IActionResult CreateDir([FromRoute]string path, [FromBody]string newDirName)
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;
            Guid? userId;
            if (authorizedUser == null) { userId = null; }
            else
            { userId = authorizedUser.Id; }

            var response = _directoryService.CreateDirectory(path, userId, newDirName);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpDelete("{*path}")]
        public IActionResult Delete([FromRoute] string path)
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;
            Guid? userId;
            if (authorizedUser == null) { userId = null; }
            else
            { userId = authorizedUser.Id; }

            var response = _directoryService.Delete(path, userId);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpPut("{*path}")]
        public IActionResult Rename([FromRoute] string path, [FromBody] string newName)
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;
            Guid? userId;
            if (authorizedUser == null) { userId = null; }
            else
            { userId = authorizedUser.Id; }

            var response = _directoryService.Rename(path, userId, newName);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

    }
}
