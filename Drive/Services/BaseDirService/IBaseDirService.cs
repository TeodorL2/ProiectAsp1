using Drive.Data.DTOs;
using Drive.Data.Enums;
using Drive.Data.Models;

namespace Drive.Services.BaseDirService
{
    public interface IBaseDirService
    {
        Task CreateBaseDirectory(string? userName, BaseDirCrUpRequestDto req);

        Task UpdateBaseDirectory(string? userName, string oldName, BaseDirCrUpRequestDto req);

        Task DeleteBaseDirectory(string? userName, string dirName);

        AccessType GetAccessType(string path, string? username);

        Task<Stream> DownloadDirOrFile(string path, string? username);

        Task UploadFiles(string path, string? username, List<IFormFile> files);

        Task<List<DirEntriesResponseDto>> GetDirectoryEntries(string path, string? username);

        Task RenameDirOrFile(string path, string? username, string newName);

        Task CreateDirectory(string path, string? username, string dirName);

        Task DeleteDirectoryOrFile(string path, string? username);

        Task DeleteAnyDirectoryOrFile(string path, string? username);

        // Access modification
        BaseDirectory? GetByDirNameAndAuthorFromPath(string path);
        void ChangeAccessToBaseDir(string path, string? username, List<string> usernames, AccessType a, bool grantOrRewoke); // true-false; grant-revoke

        // Admin specific
        Task<List<ClientSpaceUsageDto>> GetClientSpaceUsages(string? username);
    }
}
