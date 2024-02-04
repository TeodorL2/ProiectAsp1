using Drive.Data.DTOs.UserDto;
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

        [EnableCors]
        [HttpGet("{*path}")]
        public IActionResult GetEntries(string path)
        {
            var response = _directoryService.GetEntries(path);
            Console.WriteLine("DirectoryController");
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
