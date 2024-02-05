using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Drive2.Data;
using Drive2.StorageManagement;

namespace Drive.Repositories.BaseDirectoryRepository
{
    public interface IBaseDirectoryRepository : IGenericRepository<BaseDirectory>
    {
        Task<List<BaseDirectory>> FindByAuthor(Guid id);
        Task<BaseDirectory?> FindByAuthorAndRootDir(Guid id, string root);

        void UpdateIsPublic(BaseDirectory baseDirectory, bool isPublic);

        bool CreateDirectoryRoot(string dir_, Guid authorId_, string authorAccount_, bool isPublic_);

        bool DeleteRootDirectory(string dir_, string authorAccount);

        bool RenameRootDirectory(string dir_, string authorAccount, string newName);

        UserAccessToBaseDirectory GetAccessType(Guid userId, Guid dirId);


        //------------
        List<EntryStruct> GetEntries(string path);

        Task<bool> UploadFiles(string path, List<IFormFile> files);

        bool CreateDir(string path, string dirName);

        bool Delete(string path);

        bool Rename(string path, string newName);

        void UpdateUserAccessToBaseDirectory(UserAccessToBaseDirectory userAccessToBaseDirectory);

        Task<List<ClientSpaceUsageDto>> GetClientSpaceUsages();
    }
}
