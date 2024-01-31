using Drive.Data;

using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Drive.Repositories.BaseDirectoryRepository
{
    public class BaseDirectoryRepository: GenericRepository<BaseDirectory>, IBaseDirectoryRepository
    {
        public BaseDirectoryRepository(DriveContext driveContext): base(driveContext) { }

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
    }
}
