using Drive.Data.DTOs;
using Drive.Data.Enums;
using Drive.Data.Exceptions;
using Drive.Data.Models;
using Drive.Helpers.Attributes;
using Drive.Services.BaseDirService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Drive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IBaseDirService _baseDirService;

        public StorageController(IBaseDirService baseDirService)
        {
            _baseDirService = baseDirService;
        }

        [Authorize(Role.Admin, Role.User)]
        // [HttpPost("{*path}")]
        [HttpPost("create-base-dir")]
        public async Task<IActionResult> CreateBaseDir([FromBody] BaseDirCrUpRequestDto newBaseDir)
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;

            string? username = authorizedUser?.UserName;

            try
            {
                await _baseDirService.CreateBaseDirectory(username, newBaseDir);
            }
            catch(BadUser ex)
            {
                return Unauthorized(ex);
            }
            catch(NoSuchFileOrDirectory ex)
            {
                return BadRequest(ex.Message);
            }
            catch(FileAlreadyExists ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpGet("{*path}")]
        public async Task<IActionResult> GetEntries([FromRoute] string path )
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;

            string? username = authorizedUser?.UserName;
            // Console.WriteLine("Utilizator conectat: " + username);

            try
            {
                // Console.WriteLine("Calea primita: " + path);
                List<DirEntriesResponseDto> ent = await _baseDirService.GetDirectoryEntries(path, username);
                return Ok(ent);
            }
            catch(NoSuchFileOrDirectory ex)
            {
                return BadRequest(ex.Message);
            }
            catch(AccessDenied ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Role.User, Role.Admin)]
        [HttpPut("{*path}")]
        public async Task<IActionResult> UpdateBaseDir([FromRoute] string path, [FromBody] BaseDirCrUpRequestDto req)
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;

            string? username = authorizedUser?.UserName;

            string[] pathParts = path.Split('/');
            if (pathParts.Length != 2)
                return BadRequest("not a base directory");

            string baseDir = pathParts[1];

            try
            {
                await _baseDirService.UpdateBaseDirectory(username, baseDir, req);
                return Ok();
            }
            catch (NoSuchFileOrDirectory ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadUser ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Role.Admin, Role.User)]
        // [HttpPost("{*path}")]
        [HttpPost("create-dir")]
        public async Task<IActionResult> CreateDir([FromBody] CreateDirRequestDto req)
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;

            string? username = authorizedUser?.UserName;

            try
            {
                await _baseDirService.CreateDirectory(req.pathToCreateAt, username, req.DirName);
            }
            catch (BadUser ex)
            {
                return Unauthorized(ex);
            }
            catch (NoSuchFileOrDirectory ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FileAlreadyExists ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpDelete("{*path}")]
        public async Task<IActionResult> DeleteItem([FromRoute] string path)
        {
            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;

            string? username = authorizedUser?.UserName;

            try
            {
                await _baseDirService.DeleteAnyDirectoryOrFile(path, username);
                return Ok();
            }
            catch(AccessDenied ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NoSuchFileOrDirectory ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message); 
            }
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpPost("upload-files")]
        public async Task<IActionResult> UploadFiles([FromForm] UploadFilesDto req)
        {

            User? authorizedUser = HttpContext.Items["AuthorizedUser"] as User;

            string? username = authorizedUser?.UserName;

            try
            {
                await _baseDirService.UploadFiles(req.Path, username, req.Files);
                return Ok();
            }
            catch (AccessDenied ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NoSuchFileOrDirectory ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
