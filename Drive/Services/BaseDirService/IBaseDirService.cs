using Drive.Data.DTOs;
using Drive.Data.Enums;
using Drive.Data.Models;

namespace Drive.Services.BaseDirService
{
    public interface IBaseDirService
    {
        void CreateBaseDirectory(string? userName, BaseDirCrUpRequestDto req);

        void UpdateBaseDirectory(string? userName, string oldName, BaseDirCrUpRequestDto req);

        void DeleteBaseDirectory(string? userName, BaseDirCrUpRequestDto req);

        AccessType GetAccessType(string path, string? username);

        Task<Stream> DownloadDirOrFile(string path, string? username);

        Task UploadFiles(string path, string? username, List<IFormFile> files);

        List<DirEntriesResponseDto> GetDirectoryEntries(string path, string? username);

        void RenameDirOrFile(string path, string? username, string newName);

        void CreateDirectory(string path, string? username, string dirName);

        void DeleteDirectoryOrFile(string path, string? username);


        // Access modification
        BaseDirectory? GetByDirNameAndAuthorFromPath(string path);
        void ChangeAccessToBaseDir(string path, string? username, List<string> usernames, AccessType a, bool grantOrRewoke); // true-false; grant-revoke

        // Admin specific
        Task<List<ClientSpaceUsageDto>> GetClientSpaceUsages(string? username);
    }
}
