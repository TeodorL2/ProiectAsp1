using Drive.Data.Models;
using Drive2.Data;
using Drive2.Data.DTOs;
using Drive2.StorageManagement;

namespace Drive2.Services.DirectoryService
{
    public interface IDirectoryService
    {

        bool CheckIfPathIsBaseDir(string path);

        Task<BaseDirectory?> GetBaseDirByPathFromTable(string path);
        List<EntryStruct> GetEntries(string path);

        Task<int> CheckAccess(string path, Guid? userId); //1-read, 2-write, 3-rdwr

        Task<List<FileDto>> GetByPath(string path, Guid? userId);

        Task<bool> CreateDirectory(string path, Guid? userId, string dirName);

        Task<bool> Delete(string path, Guid? userId);

        Task<bool> Rename(string path, Guid? userId, string newName);

        Task<bool> UpdateIsPublic(string path, Guid? userId, bool isPublic_);

        Task SetAccessFields(Guid dirId, List<string> userNames, int readPermission_, int writePermission_); // -1 for unchanged
        Task<bool> GiveReadAccessToBaseDir(string path, Guid? userId, List<string> userNames);

        Task<bool> GiveWriteAccessToBaseDir(string path, Guid? userId, List<string> userNames);

        Task<bool> RevokeReadAccessToBaseDir(string path, Guid? userId, List<string> userNames);

        Task<bool> RevokeWriteAccessToBaseDir(string path, Guid? userId, List<string> userNames);

        Task<bool> GiveFullAccessToBaseDir(string path, Guid? userId, List<string> userNames);

        Task<bool> RevokeFullAccessToBaseDir(string path, Guid? userId, List<string> userNames);

        Task<List<ClientSpaceUsageDto>> GetClientSpaceUsages(Guid Id);
    }
}
