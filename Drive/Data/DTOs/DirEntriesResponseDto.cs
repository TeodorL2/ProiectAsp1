namespace Drive.Data.DTOs
{
    public class DirEntriesResponseDto
    {
        public string fileName { get; set; }

        public bool isDir { get; set; }

        public DateTime? dateCreated { get; set; }

        public DateTime? dateModified { get; set; }
    }
}
