using Drive.Data;

using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Drive2.Data;
using Drive2.StorageManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Drive.Repositories.BaseDirectoryRepository
{
    public class BaseDirectoryRepository : GenericRepository<BaseDirectory>, IBaseDirectoryRepository
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _storageBasePath;
        private StorageManagement _storageManagement;
        public BaseDirectoryRepository(DriveContext driveContext, IWebHostEnvironment hostEnvironment) : base(driveContext)
        {
            _hostEnvironment = hostEnvironment;
            _storageBasePath = _hostEnvironment.ContentRootPath;
            _storageBasePath = Path.Combine(_storageBasePath, "StorageManagement", "StorageRoot");
            _storageManagement = new StorageManagement(_storageBasePath);
        }

        public async Task<List<BaseDirectory>> FindByAuthor(Guid id)
        {
            return await _table.Where(t => t.Author.Equals(id)).ToListAsync();
        }
        public async Task<BaseDirectory?> FindByAuthorAndRootDir(Guid id, string root)
        {
            return await _table.FirstOrDefaultAsync(t => t.Author.Equals(id) && t.dir.Equals(root));
        }

        public void UpdateIsPublic(BaseDirectory baseDirectory, bool isPublic)
        {
            baseDirectory.IsPublic = isPublic;
            Update(baseDirectory);
        }

        public bool CreateDirectoryRoot(string dir_, Guid authorId_, string authorAccount_, bool isPublic_)
        {
            var rez = _storageManagement.CreateDir(authorAccount_, dir_);

            if(rez)
            {
                var newBaseDirectory = new BaseDirectory
                {
                    dir = dir_,
                    Author = authorId_,
                    IsPublic = isPublic_,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                };
                Create(newBaseDirectory);
            }

            return rez;
        }

        public bool DeleteRootDirectory(string dir_, string authorAccount)
        {
            _storageManagement.Delete(Path.Combine(authorAccount, dir_));
            var row = _table.FirstOrDefault(b => b.Author.Equals(authorAccount) && b.dir.Equals(dir_));
            Delete(row);
            return true;
        }

        public bool RenameRootDirectory(string dir_, string authorAccount, string newName)
        {
            var rez = _storageManagement.Rename(Path.Combine(authorAccount, dir_), newName);
            if(rez)
            {
                var row = _table.FirstOrDefault(b => b.Author.Equals(authorAccount) && b.dir.Equals(dir_));
                row.dir = newName;
                Update(row);
            }
            return rez;
        }

        

        //-----------
        public List<EntryStruct> GetEntries(string path)
        {
            return _storageManagement.GetEntries(path);
        }

        public async Task<bool> UploadFiles(string path, List<IFormFile> files)
        {
            var ok = await _storageManagement.UploadFiles(path, files);
            return ok;
        }

        public bool CreateDir(string path, string dirName)
        {
            return _storageManagement.CreateDir(path, dirName);
        }

        public bool Delete(string path)
        {
            return _storageManagement.Delete(path);
        }

        public bool Rename(string path, string newName)
        {
            return _storageManagement.Rename(path, newName);
        }

        public UserAccessToBaseDirectory GetAccessType(Guid userId, Guid dirId)
        {
            return _driveContext.UserAccessToBaseDirectorys.FirstOrDefault(t => t.UserId.Equals(userId) && t.BaseDirectoryId.Equals(dirId));
        }

        public void UpdateUserAccessToBaseDirectory(UserAccessToBaseDirectory userAccessToBaseDirectory)
        {
            _driveContext.UserAccessToBaseDirectorys.Update(userAccessToBaseDirectory);
        }

        public async Task<List<ClientSpaceUsageDto>> GetClientSpaceUsages()
        {
            var rez = await _driveContext.BaseDirectorys.Join(_driveContext.Users, b => b.Author, u => u.Id,
                (b, u) => new { b, u }).GroupBy(j => j.u.Username)
                .Select(d => new
                {
                    userN = d.Key,
                    count = d.Select(x => x.b.dir).Count()
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
