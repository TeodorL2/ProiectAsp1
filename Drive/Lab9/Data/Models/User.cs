using Drive.Data.Models.Base;
using Drive.Data.Enums;

namespace Drive.Data.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public ICollection<BaseDirectory>? BaseDirectorys { get; set; }
        public ICollection<UserAccessToBaseDirectory>? UserAccessToBaseDirectorys { get; set; }
    }
}
