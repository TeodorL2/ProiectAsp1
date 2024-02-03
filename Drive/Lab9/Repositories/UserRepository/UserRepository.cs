using Drive.Data;

using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Drive.Repositories.UserRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DriveContext driveContext) : base(driveContext) { }

        public async Task<List<User>> FindAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<User> FindByUsername(string username)
        {
            return await _table.FirstOrDefaultAsync(u => u.Username.Equals(username));
        }
    }
}
