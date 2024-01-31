using Drive.Data.Models;
using Drive.Repositories.BaseDirectoryRepository;
using Drive.Repositories.DirectoryDescRepository;
using Drive.Repositories.UserRepository;

namespace Drive.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        UserRepository UserRepository { get; }

        BaseDirectoryRepository BaseDirectoryRepository { get; }

        DirectoryDescRepository DirectoryDescRepository { get; }

        bool Save();
        Task<bool> SaveAsync();
    }
}
