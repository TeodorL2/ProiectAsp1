using Drive.Data.Models.Base;

namespace Drive.Data.Models
{
    public class DirectoryDesc: BaseEntity
    {
        public string description { get; set; }

        public Guid DirectoryId { get; set; }

        public BaseDirectory BaseDirectory { get; set; }
    }
}
