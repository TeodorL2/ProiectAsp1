using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Drive2.Data.DTOs.BaseDirectoryDto;

namespace Drive.Repositories.UserRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> FindByUsername(string username);

        Task<List<User>> FindAll();

        Task<List<BaseDirectoryDto>> GetAllWithPermission(Guid userId);
    }
}
