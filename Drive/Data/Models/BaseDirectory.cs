using Drive.Data.Models.Base;

namespace Drive.Data.Models
{
    public class BaseDirectory: BaseEntity
    {
        public string DirectoryName { get; set; }
        public Guid Author { get; set; }
        public bool IsPublic { get; set; }

        public ICollection<UserAccessToBaseDir>? UserAccessToBaseDirs { get; set; }
        public User User { get; set; }
        public BaseDirDescription BaseDirDescription { get; set; }
    }
}
