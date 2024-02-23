using Drive.Data;
using Drive.Data.DTOs;
using Drive.Data.Enums;
using Drive.Data.Exceptions;
using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Drive.Services.FileSystemService;
using Drive.StorageManagement.StorageManagement;
using Microsoft.EntityFrameworkCore;

namespace Drive.Repositories.BaseDirectoryRepository
{
    public class BaseDirectoryRepository: GenericRepository<BaseDirectory>, IBaseDirectoryRepository
    {
        // private readonly IWebHostEnvironment _hostEnvironment;
        // private readonly string _storageBasePath;
        // private StorageManagement.StorageManagement.StorageManagement _storageManagement;

        private readonly IFileSystemService _fileSystemService;
        public BaseDirectoryRepository(DriveContext driveContext, IFileSystemService fileSystemService) : base(driveContext)
        {
            // _hostEnvironment = hostEnvironment;
            // _storageBasePath = _hostEnvironment.ContentRootPath;
            // _storageBasePath = Path.Combine(_storageBasePath, "StorageManagement", "StorageRoot");
            // _storageManagement = new StorageManagement.StorageManagement.StorageManagement(_storageBasePath);
            _fileSystemService = fileSystemService;
        }

        public async Task CreateBaseDirectory(string path, BaseDirectory baseDirectory)
        {
            await _fileSystemService.CreateDir(path, baseDirectory.DirectoryName);

            Create(baseDirectory);
        }

        public async Task UpdateBaseDirectory(string path, BaseDirectory baseDirectory)
        {
            await _fileSystemService.Rename(path, baseDirectory.DirectoryName);

            Update(baseDirectory);
        }

        public async Task DeleteBaseDirectory(string path, BaseDirectory baseDirectory)
        {
            await _fileSystemService.Delete(path);

            Delete(baseDirectory);
        }

        public BaseDirectory? GetByDirNameAndAuthor(string dirname, Guid author)
        {
            return _table.FirstOrDefault(t => t.DirectoryName.Equals(dirname) && t.Author.Equals(author));
        }

        public BaseDirectory? GetByDirName(string dirname)
        {
            return _table.FirstOrDefault(t => t.DirectoryName.Equals(dirname));
        }

        public UserAccessToBaseDir? GetAccessType(Guid userId, Guid baseDirId)
        {
            return _driveContext.UserAccessToBaseDirs.FirstOrDefault(t => t.UserId.Equals(userId) && t.BaseDirId.Equals(baseDirId));
        }

        public async Task<Stream> DownloadDirOrFile(string path)
        {
            return await _fileSystemService.DownloadDirOrFile(path);
            return null;
        }

        public async Task UploadFiles(string path, List<IFormFile> files)
        {
            await _fileSystemService.UploadFiles(path, files);
        }

        public async Task<List<EntryStruct>> GetEntries(string path)
        {
            return await _fileSystemService.GetEntries(path);
        }

        public async Task RenameDirOrFile(string path, string newName)
        {
            await _fileSystemService.Rename(path, newName);
        }

        public async Task CreateDirectory(string path, string dirName)
        {
            await _fileSystemService.CreateDir(path, dirName);
        }

        public async Task DeleteDirectoryOrFile(string path)
        {
            await _fileSystemService.Delete(path);
        }

        public async Task CreateUserRootDir(string username)
        {
            await _fileSystemService.CreateDir("", username);
        }

        public async Task DeleteUserRootDir(string username)
        {
            await _fileSystemService.Delete(username);
        }

        public void ChangeAccessType(Guid userId, Guid baseDirId, AccessType accessType, bool grantOrRevoke)
        {
            UserAccessToBaseDir? rowToUpdate = _driveContext.UserAccessToBaseDirs.FirstOrDefault(t => t.UserId.Equals(userId) && t.BaseDirId.Equals(baseDirId));

            UserAccessToBaseDir userAccessToBaseDir = new UserAccessToBaseDir{
                UserId = userId,
                BaseDirId = baseDirId,
                AccessType = AccessType.NoAccess
            };

            if (rowToUpdate == null)
            {
                if(grantOrRevoke)
                {
                    userAccessToBaseDir.AccessType |= accessType;
                    _driveContext.Add(userAccessToBaseDir);
                    return;
                }
                
            }
            else
            {
                if(grantOrRevoke)
                {
                    userAccessToBaseDir.AccessType = rowToUpdate.AccessType;
                    userAccessToBaseDir.AccessType |= accessType;
                    _driveContext.Update(userAccessToBaseDir);
                    return;
                }
                else
                {
                    userAccessToBaseDir.AccessType = rowToUpdate.AccessType;
                    userAccessToBaseDir.AccessType &= ~accessType;
                    _driveContext.Update(userAccessToBaseDir);
                    return;
                }
            }
        }


        public async Task<List<ClientSpaceUsageDto>> GetClientSpaceUsages()
        {
            var rez = await _driveContext.BaseDirectories.Join(_driveContext.Users, b => b.Author, u => u.Id,
                (b, u) => new { b, u }).GroupBy(j => j.u.UserName)
                .Select(d => new
                {
                    userN = d.Key,
                    count = d.Select(x => x.b.DirectoryName).Count()
                }).ToListAsync();

            var resp = new List<ClientSpaceUsageDto>();
            foreach (var r in rez)
            {
                resp.Add(new ClientSpaceUsageDto { userName = r.userN, nrBaseFolders = r.count });
            }

            return resp;
        }

    }
}
