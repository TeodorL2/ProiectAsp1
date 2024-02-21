using Drive.Repositories.BaseDirectoryRepository;
using Drive.Repositories.UserRepository;

namespace Drive.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }

        IBaseDirectoryRepository BaseDirectoryRepository { get; }

        bool Save();
        Task<bool> SaveAsync();
    }
}
