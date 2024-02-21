using Drive.Data;
using Drive.Data.Models;
using Drive.Repositories.GenericRepository;

namespace Drive.Repositories.UserRepository
{
    public class UserRepository: GenericRepository<User>, IUserRepository
    {
        public UserRepository(DriveContext driveContext): base(driveContext) { }
        public User? FindByUserName(string? userName)
        {
            if (userName == null)
                return null;
            return _table.FirstOrDefault(t => t.UserName.Equals(userName));  //// ?????????????
        }
    }
}
