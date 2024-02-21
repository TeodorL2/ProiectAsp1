using Drive.Data.Models.Base;

namespace Drive.Data.Models
{
    public class BaseDirDescription: BaseEntity
    {
        string Description { get; set; }
        public Guid BaseDirId { get; set; }

        public BaseDirectory BaseDirectory { get; set; }
    }
}
