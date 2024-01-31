using Drive.Data.Models;
using Drive.Repositories.GenericRepository;

namespace Drive.Repositories.BaseDirectoryRepository
{
    public interface IBaseDirectoryRepository: IGenericRepository<BaseDirectory>
    {
        Task<List<BaseDirectory>> FindByAuthor(Guid id);
        Task<BaseDirectory> FindByAuthorAndRootDir(Guid id, string root);

        void UpdateIsPublic(BaseDirectory baseDirectory, bool isPublic);
    }
}
