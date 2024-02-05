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
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSpecificController : ControllerBase
    {
        private readonly IDirectoryService _directoryService;

        public AdminSpecificController(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }

        [Authorize(Role.Admin)]
        [HttpGet("space-usage")]
        public async Task<IActionResult> GetSpaceUsegePerClient()
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;
            Guid? userId;
            if (authorizedUser == null) { userId = null; }
            else
            { userId = authorizedUser.Id; }
            if(userId  == null) { return BadRequest(); }
            var resp = await _directoryService.GetClientSpaceUsages((Guid)userId);

            return Ok(resp);
        }
    }
}
