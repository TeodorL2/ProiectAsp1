using Drive.Data.Enums;
using Drive.Data.Exceptions;
using Drive.Data.Models;
using Drive.Helpers.Attributes;
using Drive.Services.BaseDirService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Drive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSpecificController : ControllerBase
    {
        private readonly IBaseDirService _baseDirService;

        public AdminSpecificController(IBaseDirService baseDirService)
        {
            _baseDirService = baseDirService;
        }

        [Authorize(Role.Admin)]
        [HttpGet("space-usage")]
        public async Task<IActionResult> GetSpaceUsegePerClient()
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;

            string? username = authorizedUser?.UserName;

            try
            {
                var resp = _baseDirService.GetClientSpaceUsages(username);
                return Ok(resp);
            }
            catch (BadUser ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
