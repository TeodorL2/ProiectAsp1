namespace Drive2.Data.DTOs
{
    public class FileDto
    {
        public string fileName { get; set; }

        public bool isDir { get; set; }

        public DateTime? dateCreated { get; set; }

        public DateTime? dateModified { get; set; }

        public bool HasReadPermission { get; set; }

        public bool HasWritePermission { get; set;}
    }
}
