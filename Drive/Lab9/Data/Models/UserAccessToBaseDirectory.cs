namespace Drive.Data.Models
{
    public class UserAccessToBaseDirectory
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid BaseDirectoryId { get; set; }

        public BaseDirectory BaseDirectory { get; set; }
        public bool HasReadPermission { get; set; }

        public bool HasWritePermission { get; set; }
    }
}
