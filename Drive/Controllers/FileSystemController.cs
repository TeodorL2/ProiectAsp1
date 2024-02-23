using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Drive.StorageManagement.StorageManagement;
using Drive.Data;
using Drive.Data.Exceptions;
using System.Net;
using Drive.StorageManagement.StorageManagement.DTOs;

namespace Drive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileSystemController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _storageBasePath;
        private StorageManagement.StorageManagement.StorageManagement _storageManagement;
        public FileSystemController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _storageBasePath = _hostEnvironment.ContentRootPath;
            _storageBasePath = Path.Combine(_storageBasePath, "StorageManagement", "StorageRoot");
            _storageManagement = new StorageManagement.StorageManagement.StorageManagement(_storageBasePath);
        }

        [HttpPost("get-entries")]
        public IActionResult GetEntries([FromBody] string path)
        {
            try
            {
                List<EntryStruct>? resp = _storageManagement.GetEntries(path);
                return Ok(resp);
            }
            catch (NoSuchFileOrDirectory ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-dir")]
        public IActionResult CreateDir([FromBody] CreateDirDto req)
        {
            try
            {
                _storageManagement.CreateDir(req.Path, req.DirName);
                return Ok();
            }
            catch (NoSuchFileOrDirectory ex)
            {
                return NotFound(ex.Message);
            }
            catch (FileAlreadyExists ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete-item")]
        public IActionResult Delete([FromBody] string path)
        {
            try
            {
                _storageManagement.Delete(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload-files")]
        public async Task<IActionResult> UploadFiles([FromBody] UploadFilesDto req)
        {
            try
            {
                await _storageManagement.UploadFiles(req.Path, req.Files);
                return Ok();
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

        [HttpPost("download-items")]
        public async Task<IActionResult> DownloadDirOrFile([FromBody] string path)
        {
            try
            {
                var resp = await _storageManagement.DownloadDirOrFile(path);
                return Ok(resp);
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

        [HttpPost("rename-item")]
        public IActionResult Rename([FromBody] RenameItemDto req)
        {
            try
            {
                _storageManagement.Rename(req.Path, req.NewName);
                return Ok();
            }
            catch (NoSuchFileOrDirectory ex)
            {
                return NotFound(ex.Message);
            }
            catch (FileAlreadyExists ex)
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
