using Drive.Data.Models;

namespace Drive2.Data.DTOs.BaseDirectoryDto
{
    public class BaseDirectoryDto
    {
        public string dir { get; set; }

        public bool IsPublic { get; set; }

        public Guid Author { get; set; } // user id

        public bool hasReadPermission { get; set; }

        public bool hasWritePermission { get; set;}
    }
}
