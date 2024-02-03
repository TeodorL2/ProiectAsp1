using Drive.Data.Models.Base;

namespace Drive.Data.Models
{
    public class BaseDirectory : BaseEntity
    {
        public string dir { get; set; }

        public bool IsPublic { get; set; }

        public Guid Author { get; set; } // user id
        public User User { get; set; }

        public ICollection<UserAccessToBaseDirectory>? UserAccessToBaseDirectorys { get; set; }

        public DirectoryDesc DirectoryDesc { get; set; }
    }
}
