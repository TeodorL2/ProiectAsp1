using Drive.Data.DTOs;
using Drive.Data.Enums;
using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Drive.StorageManagement.StorageManagement;

namespace Drive.Repositories.BaseDirectoryRepository
{
    public interface IBaseDirectoryRepository: IGenericRepository<BaseDirectory>
    {
        Task CreateBaseDirectory(string path, BaseDirectory baseDirectory);
        Task UpdateBaseDirectory(string path, BaseDirectory baseDirectory);
        Task DeleteBaseDirectory(string path, BaseDirectory baseDirectory);

        BaseDirectory? GetByDirNameAndAuthor(string dirname, Guid author);

        BaseDirectory? GetByDirName(string dirname);

        UserAccessToBaseDir? GetAccessType(Guid userId, Guid baseDirId);

        Task<Stream> DownloadDirOrFile(string path);

        Task UploadFiles(string path, List<IFormFile> files);

        Task<List<EntryStruct>> GetEntries(string path);

        Task RenameDirOrFile(string path, string newName);

        Task CreateDirectory(string path, string dirName);

        Task DeleteDirectoryOrFile(string path);

        // at user register
        Task CreateUserRootDir(string username);
        Task DeleteUserRootDir(string username);

        void ChangeAccessType(Guid userId, Guid baseDirId, AccessType accessType, bool grantOrRevoke);

        // admin specific
        Task<List<ClientSpaceUsageDto>> GetClientSpaceUsages();
    }
}
