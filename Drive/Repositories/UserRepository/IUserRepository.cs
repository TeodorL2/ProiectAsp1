using Drive.Data.Models;
using Drive.Repositories.GenericRepository;

namespace Drive.Repositories.UserRepository
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User> FindByUsername(string username);

        Task<List<User>> FindAll();
    }
}
