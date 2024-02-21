using Drive.Data.Enums;
using Drive.Data.Models.Base;
using System.Text.Json.Serialization;

namespace Drive.Data.Models
{
    public class User: BaseEntity
    {
        public string UserName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public Role Role { get; set; }

        public ICollection<UserAccessToBaseDir>? UserAccessToBaseDirs { get; set; }
        public ICollection<BaseDirectory>? BaseDirectories { get; set; }
    }
}
