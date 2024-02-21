namespace Drive.StorageManagement.StorageManagement
{
    public class EntryStruct
    {
        public string fileName { get; set; }

        public bool isDir { get; set; }

        public DateTime? dateCreated { get; set; }

        public DateTime? dateModified { get; set; }

        public EntryStruct(string fileName, bool isDir, DateTime? dateCreated, DateTime? dateModified)
        {
            this.fileName = fileName;
            this.isDir = isDir;
            this.dateCreated = dateCreated;
            this.dateModified = dateModified;
        }
    }
}
