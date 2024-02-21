using Drive.Data.Models;
using Drive.Repositories.GenericRepository;

namespace Drive.Repositories.UserRepository
{
    public interface IUserRepository: IGenericRepository<User>
    {
        User? FindByUserName(string? userName);
    }
}
