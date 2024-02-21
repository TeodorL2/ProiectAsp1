using Drive.Data.Enums;

namespace Drive.Data.Models
{
    public class UserAccessToBaseDir
    {
        public Guid UserId { get; set; }
        public Guid BaseDirId { get; set; }

        public AccessType AccessType { get; set; }

        public User User { get; set; }
        public BaseDirectory BaseDirectory { get; set; }
    }
}
