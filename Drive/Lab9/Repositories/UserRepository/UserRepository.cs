using Drive.Data;

using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Drive2.Data.DTOs.BaseDirectoryDto;
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
            var temp = await _table.FirstOrDefaultAsync(u => u.Username.Equals(username));
            if(temp == null) {
                Console.WriteLine("Userul nu a fost gasit!!!!!!!!!!!!!!!!!!!!");
            }
            return temp;
        }

        public async Task<List<BaseDirectoryDto>> GetAllWithPermission(Guid userId)
        {
            var rez = await _driveContext.UserAccessToBaseDirectorys.Join(_driveContext.BaseDirectorys,
                ua => ua.BaseDirectoryId, b => b.Id,
                (ua, b) => new { ua, b })
                .Where(j => j.ua.UserId.Equals(userId))
                .Select(j => new
                {
                    j.b.dir, j.b.Author, j.b.IsPublic, j.ua.HasReadPermission, j.ua.HasWritePermission
                }).ToListAsync();

            var resp = new List<BaseDirectoryDto> ();

            foreach (var i in rez)
            {
                var dto = new BaseDirectoryDto
                {
                    dir = i.dir,
                    Author = i.Author,
                    IsPublic = i.IsPublic,
                    hasReadPermission = i.HasReadPermission,
                    hasWritePermission = i.HasWritePermission,
                };
                resp.Add(dto);
            }
            return resp;
        }
    }
}
