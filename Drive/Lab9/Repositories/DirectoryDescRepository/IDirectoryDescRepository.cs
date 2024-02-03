using Drive.Data.Models;
using Drive.Repositories.GenericRepository;

namespace Drive.Repositories.DirectoryDescRepository
{
    public interface IDirectoryDescRepository : IGenericRepository<DirectoryDesc>
    {
        void UpdateDescription(DirectoryDesc directoryDesc, string desc);
    }
}
