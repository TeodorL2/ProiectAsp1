using Drive.Data.Models;

namespace Drive.Data.DTOs
{
    public class BaseDirCrUpRequestDto
    {
        public string DirectoryName { get; set; }
        public bool IsPublic { get; set; }
    }
}
