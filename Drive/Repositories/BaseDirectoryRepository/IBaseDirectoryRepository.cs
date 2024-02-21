using Drive.Data.DTOs;
using Drive.Data.Enums;
using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Drive.StorageManagement.StorageManagement;

namespace Drive.Repositories.BaseDirectoryRepository
{
    public interface IBaseDirectoryRepository: IGenericRepository<BaseDirectory>
    {
        void CreateBaseDirectory(string path, BaseDirectory baseDirectory);
        void UpdateBaseDirectory(string path, BaseDirectory baseDirectory);
        void DeleteBaseDirectory(string path, BaseDirectory baseDirectory);

        BaseDirectory? GetByDirNameAndAuthor(string dirname, Guid author);

        BaseDirectory? GetByDirName(string dirname);

        UserAccessToBaseDir? GetAccessType(Guid userId, Guid baseDirId);

        Task<Stream> DownloadDirOrFile(string path);

        Task UploadFiles(string path, List<IFormFile> files);

        List<EntryStruct> GetEntries(string path);

        void RenameDirOrFile(string path, string newName);

        void CreateDirectory(string path, string dirName);

        void DeleteDirectoryOrFile(string path);

        // at user register
        void CreateUserRootDir(string username);
        void DeleteUserRootDir(string username);

        void ChangeAccessType(Guid userId, Guid baseDirId, AccessType accessType, bool grantOrRevoke);

        // admin specific
        Task<List<ClientSpaceUsageDto>> GetClientSpaceUsages();
    }
}
