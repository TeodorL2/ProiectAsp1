using Drive.Data;

using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Drive2.StorageManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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

            if (_hostEnvironment == null)
                Console.WriteLine("environment not injected!!!!!!!!!!!!!!!!!!!!!!!!!");

            _storageBasePath = _hostEnvironment.ContentRootPath;
            _storageBasePath = Path.Combine(_storageBasePath, "StorageManagement", "StorageRoot");
            _storageManagement = new StorageManagement(_storageBasePath);
        }

        public async Task<List<BaseDirectory>> FindByAuthor(Guid id)
        {
            return await _table.Where(t => t.Author.Equals(id)).ToListAsync();
        }
        public async Task<BaseDirectory> FindByAuthorAndRootDir(Guid id, string root)
        {
            return await _table.FirstOrDefaultAsync(t => t.Author.Equals(id) && t.dir.Equals(root));
        }

        public void UpdateIsPublic(BaseDirectory baseDirectory, bool isPublic)
        {
            baseDirectory.IsPublic = isPublic;
            Update(baseDirectory);
        }

        public List<EntryStruct> GetEntries(string path)
        {
            return _storageManagement.GetEntries(path);
        }
    }
}
