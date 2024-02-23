using Drive.StorageManagement.StorageManagement;

namespace Drive.Services.FileSystemService
{
    public interface IFileSystemService
    {
        Task<List<EntryStruct>> GetEntries(string path);
        Task CreateDir(string path, string dirName);

        Task Delete(string path);

        Task UploadFiles(string path, List<IFormFile> files);
        Task<Stream> DownloadDirOrFile(string path);

        Task Rename(string path, string newName);
    }
}
