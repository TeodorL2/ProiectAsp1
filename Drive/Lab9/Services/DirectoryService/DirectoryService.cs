using Drive.Data.Models;
using Drive.Helpers.JwtUtil;
using Drive.UnitOfWork;
using Drive2.Data;
using Drive2.Data.DTOs;
using Drive2.StorageManagement;
using Drive.Data.Enums;

namespace Drive2.Services.DirectoryService
{
    public class DirectoryService: IDirectoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DirectoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CheckIfPathIsBaseDir(string path)
        {
            char separator = Path.DirectorySeparatorChar;
            string[] pathParts = Path.GetFullPath(path).Split(separator);
            if (pathParts.Length != 2) return false;

            return true;
        }

        public async Task<BaseDirectory?> GetBaseDirByPathFromTable(string path)
        {
            char separator = Path.DirectorySeparatorChar;
            string[] pathParts = Path.GetFullPath(path).Split(separator);
            if (pathParts.Length != 2) return null;

            User author = await _unitOfWork.UserRepository.FindByUsername(pathParts[0]);
            Guid authorId = author.Id;
            BaseDirectory? baseDirectory = await _unitOfWork.BaseDirectoryRepository.FindByAuthorAndRootDir(authorId, pathParts[1]);
            return baseDirectory;
        }

        public List<EntryStruct> GetEntries(string path)
        {
            Console.WriteLine("DirectoryService: " + path);
            return _unitOfWork.BaseDirectoryRepository.GetEntries(path);
        }


        public async Task<int> CheckAccess(string path, Guid? userId)
        {

            char separator = Path.DirectorySeparatorChar;
            string[] pathParts = Path.GetFullPath(path).Split(separator);

            if (pathParts.Length > 1)
            {
                string authorAccount = pathParts[0];
                string rootDir = pathParts[1];

                var auth = await _unitOfWork.UserRepository.FindByUsername(authorAccount);
                var dir = await _unitOfWork.BaseDirectoryRepository.FindByAuthorAndRootDir(auth.Id, rootDir);
                if (dir == null) return 0;

                var dirId = dir.Id;

                if (dir.IsPublic)
                {
                    return 3;
                }
                else

                if (userId != null)
                {
                    var accessType = _unitOfWork.BaseDirectoryRepository.GetAccessType((Guid)userId, dirId);
                    if (accessType != null)
                    {
                        int resp = 0;
                        if (accessType.HasReadPermission) resp = resp | 1;
                        if (accessType.HasWritePermission) resp = resp | 2;
                        return resp;
                    }
                }
                
            }
            
            return 0;
        }

        public async Task<List<FileDto>> GetByPath(string path, Guid? userId)
        {
            var resp = new List<FileDto>();

            var hasAccess = await CheckAccess(path, userId);
            if (hasAccess != 0)
            {
                var entries = _unitOfWork.BaseDirectoryRepository.GetEntries(path);
                foreach (var entry in entries)
                {
                    bool readPermission = (hasAccess & 1) > 0;
                    bool writePermission = (hasAccess & 2) > 0;
                    var dto = new FileDto
                    {
                        fileName = entry.fileName,
                        isDir = entry.isDir,
                        dateCreated = entry.dateCreated,
                        dateModified = entry.dateModified,
                        HasReadPermission = readPermission,
                        HasWritePermission = writePermission
                    };
                    resp.Add(dto);
                }
            }

            return resp;
        }

        public async Task<bool> CreateDirectory(string path, Guid? userId, string dirName)
        {
            var access = await CheckAccess(path, userId);
            if((access & 2) == 0) return false;

            return _unitOfWork.BaseDirectoryRepository.CreateDir(path, dirName);
            
        }

        public async Task<bool> Delete(string path, Guid? userId)
        {
            var access = await CheckAccess(path, userId);
            if ((access & 2) == 0) return false;

            return _unitOfWork.BaseDirectoryRepository.Delete(path);
        }

        public async Task<bool> Rename(string path, Guid? userId, string newName)
        {
            var access = await CheckAccess(path, userId);
            if ((access & 2) == 0) return false;

            return _unitOfWork.BaseDirectoryRepository.Rename(path, newName);
        }

        public async Task<bool> UpdateIsPublic(string path, Guid? userId, bool isPublic_)
        {
            var access = await CheckAccess(path, userId);
            if ((access & 2) == 0) return false;

            BaseDirectory? baseDirectory = await GetBaseDirByPathFromTable(path);

            if (baseDirectory == null) return false;

            _unitOfWork.BaseDirectoryRepository.UpdateIsPublic(baseDirectory, isPublic_);

            return true;
        }

        public async Task SetAccessFields(Guid dirId, List<string> userNames, int readPermission_, int writePermission_)
        {
            List<User> users = new List<User>();
            foreach (var user in userNames)
            {
                var userTemp = await _unitOfWork.UserRepository.FindByUsername(user);
                users.Add(userTemp);
            }

            foreach(var user in users)
            {
                var row = _unitOfWork.BaseDirectoryRepository.GetAccessType(user.Id, dirId);
                if (row == null) continue;
                if(readPermission_ != -1)
                    row.HasReadPermission = readPermission_ > 0;
                if(writePermission_ != -1)
                row.HasWritePermission = writePermission_ > 0;
                _unitOfWork.BaseDirectoryRepository.UpdateUserAccessToBaseDirectory(row);
            }
        }
        public async Task<bool> GiveReadAccessToBaseDir(string path, Guid? userId, List<string> userNames)
        {
            var access = await CheckAccess(path, userId);
            if ((access & 2) == 0) return false;

            BaseDirectory? baseDirectory = await GetBaseDirByPathFromTable(path);

            if (baseDirectory == null) return false;

            await SetAccessFields(baseDirectory.Id, userNames, 1, -1);

            return true;
        }

        public async Task<bool> GiveWriteAccessToBaseDir(string path, Guid? userId, List<string> userNames)
        {
            var access = await CheckAccess(path, userId);
            if ((access & 2) == 0) return false;

            BaseDirectory? baseDirectory = await GetBaseDirByPathFromTable(path);

            if (baseDirectory == null) return false;

            await SetAccessFields(baseDirectory.Id, userNames, -1, 1);

            return true;
        }

        public async Task<bool> RevokeReadAccessToBaseDir(string path, Guid? userId, List<string> userNames)
        {
            var access = await CheckAccess(path, userId);
            if ((access & 2) == 0) return false;

            BaseDirectory? baseDirectory = await GetBaseDirByPathFromTable(path);

            if (baseDirectory == null) return false;

            await SetAccessFields(baseDirectory.Id, userNames, 0, -1);

            return true;
        }

        public async Task<bool> RevokeWriteAccessToBaseDir(string path, Guid? userId, List<string> userNames)
        {
            var access = await CheckAccess(path, userId);
            if ((access & 2) == 0) return false;

            BaseDirectory? baseDirectory = await GetBaseDirByPathFromTable(path);

            if (baseDirectory == null) return false;

            await SetAccessFields(baseDirectory.Id, userNames, -1, 0);

            return true;
        }

        public async Task<bool> GiveFullAccessToBaseDir(string path, Guid? userId, List<string> userNames)
        {
            var access = await CheckAccess(path, userId);
            if ((access & 2) == 0) return false;

            BaseDirectory? baseDirectory = await GetBaseDirByPathFromTable(path);

            if (baseDirectory == null) return false;

            await SetAccessFields(baseDirectory.Id, userNames, 1, 1);

            return true;
        }

        public async Task<bool> RevokeFullAccessToBaseDir(string path, Guid? userId, List<string> userNames)
        {
            var access = await CheckAccess(path, userId);
            if ((access & 2) == 0) return false;

            BaseDirectory? baseDirectory = await GetBaseDirByPathFromTable(path);

            if (baseDirectory == null) return false;

            await SetAccessFields(baseDirectory.Id, userNames, 0, 0);

            return true;
        }

        public async Task<List<ClientSpaceUsageDto>> GetClientSpaceUsages(Guid Id)
        {
            var resp = new List<ClientSpaceUsageDto>();

            User? user = _unitOfWork.UserRepository.FindById(Id);
            if (user == null) return null;
            if (user.Role != Role.Admin) return null;

            resp = await _unitOfWork.BaseDirectoryRepository.GetClientSpaceUsages();

            return resp;
        }
    }
}
