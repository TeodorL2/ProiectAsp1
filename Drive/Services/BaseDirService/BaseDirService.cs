using Drive.Data.DTOs;
using Drive.Data.Enums;
using Drive.Data.Exceptions;
using Drive.Data.Models;
using Drive.StorageManagement.StorageManagement;
using Drive.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO.Compression;
using System.Xml;

namespace Drive.Services.BaseDirService
{
    public class BaseDirService: IBaseDirService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseDirService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void CreateBaseDirectory(string? userName, BaseDirCrUpRequestDto req)
        {
            var user = _unitOfWork.UserRepository.FindByUserName(userName);
            if (user == null)
            {
                throw new BadUser("user not found by username");
            }

            BaseDirectory baseDirToCreate = new BaseDirectory
            {
                DirectoryName = user.UserName + "_" + req.DirectoryName,
                Author = user.Id,
                IsPublic = req.IsPublic
            };
            _unitOfWork.BaseDirectoryRepository.CreateBaseDirectory(userName, baseDirToCreate);

            _unitOfWork.Save();
        }

        public void UpdateBaseDirectory(string? userName, string oldName, BaseDirCrUpRequestDto req)
        {
            var user = _unitOfWork.UserRepository.FindByUserName(userName);
            if (user == null)
            {
                throw new BadUser("user not found by username");
            }

            var baseDir = _unitOfWork.BaseDirectoryRepository.GetByDirNameAndAuthor(oldName, user.Id);

            if (baseDir == null)
                throw new NoSuchFileOrDirectory("path not found");

            baseDir.DirectoryName = req.DirectoryName;
            baseDir.IsPublic = req.IsPublic;

            /*BaseDirectory baseDirNew = new BaseDirectory
            {
                DirectoryName = req.DirectoryName,
                Author = user.Id,
                IsPublic = req.IsPublic
            };*/
            string path = Path.Combine(userName, oldName);
            _unitOfWork.BaseDirectoryRepository.UpdateBaseDirectory(path, baseDir);

            _unitOfWork.Save();
        }

        public void DeleteBaseDirectory(string? userName, BaseDirCrUpRequestDto req)
        {
            var user = _unitOfWork.UserRepository.FindByUserName(userName);
            if (user == null)
            {
                throw new BadUser("user not found by username");
            }

            var baseDir = _unitOfWork.BaseDirectoryRepository.GetByDirNameAndAuthor(req.DirectoryName, user.Id);

            if (baseDir == null) return;
                // throw new NoSuchFileOrDirectory("path not found");

            string path = Path.Combine(userName, req.DirectoryName);
            _unitOfWork.BaseDirectoryRepository.DeleteBaseDirectory(path, baseDir);

            _unitOfWork.Save();
        }

        public AccessType GetAccessType(string path, string? username)
        {
            char separator = Path.DirectorySeparatorChar;
            // string[] pathParts = Path.GetFullPath(path).Split(separator);
            string[] pathParts = path.Split('/');

            Console.WriteLine("Cale de validat: " + path);

            var user = _unitOfWork.UserRepository.FindByUserName(username);
            // if (user == null)
            //     throw new BadUser("user not found by username");

            if (pathParts.Length <= 0)
            {
                Console.WriteLine("***Path length***");
                throw new NoSuchFileOrDirectory("invalid path");
            }
            else if (pathParts.Length <= 1)
            {
                if (user == null)
                    return AccessType.NoAccess;

                /*
                var baseDir = _unitOfWork.BaseDirectoryRepository.GetByDirNameAndAuthor(pathParts[0], user.Id);
                if (baseDir == null)
                    return AccessType.NoAccess;
                else
                    return AccessType.FullAccess;
                */
                if (pathParts[0] == user.UserName)
                    return AccessType.FullAccess;
                else
                    return AccessType.NoAccess;
            }
            else
            {
                AccessType a = AccessType.NoAccess;
                string userDir = pathParts[0];
                string baseDir = pathParts[1];

                User? authorUser = _unitOfWork.UserRepository.FindByUserName(userDir);
                if (authorUser == null)
                {
                    // Console.WriteLine("***Author***" + userDir);
                    throw new NoSuchFileOrDirectory("invalid path");
                }

                BaseDirectory? baseDirFromTable = _unitOfWork.BaseDirectoryRepository.GetByDirName(baseDir);

                if (baseDirFromTable == null)
                {
                    // Console.WriteLine("***database table***");
                    throw new NoSuchFileOrDirectory("invalid path");
                }

                if (user.Id == baseDirFromTable.Author)
                    a |= AccessType.FullAccess;

                if (baseDirFromTable.IsPublic)
                    a |= AccessType.Read;

                if (user != null)
                {
                    var userAccessToBaseDir = _unitOfWork.BaseDirectoryRepository.GetAccessType(user.Id, baseDirFromTable.Id);
                    if (userAccessToBaseDir != null)
                        a |= userAccessToBaseDir.AccessType;
                }

                return a;

            }
        }

        public bool PathIsNotUserDir(string path)
        {
            char separator = Path.DirectorySeparatorChar;
            string[] pathParts = Path.GetFullPath(path).Split(separator);
            return pathParts.Length > 1;
        }

        public bool PathIsNotBaseDir(string path)
        {
            char separator = Path.DirectorySeparatorChar;
            string[] pathParts = Path.GetFullPath(path).Split(separator);
            return pathParts.Length > 2;
        }

        public async Task<Stream> DownloadDirOrFile(string path, string? username)
        {
            var accessType = GetAccessType(path, username);
            if ((AccessType.Read & accessType) == 0)
                throw new AccessDenied("no read privilege");


            return await _unitOfWork.BaseDirectoryRepository.DownloadDirOrFile(path);
        }

        public async Task UploadFiles(string path, string? username, List<IFormFile> files)
        {
            var accessType = GetAccessType(path, username);
            if ((AccessType.Write & accessType) == 0)
                throw new AccessDenied("no write privilege");
            if(PathIsNotUserDir(path) == false)
                throw new AccessDenied("only folders can be created in user root directory");

            await _unitOfWork.BaseDirectoryRepository.UploadFiles(path, files);
        }

        public List<DirEntriesResponseDto> GetDirectoryEntries(string path, string? username)
        {
            List<DirEntriesResponseDto> response = new List<DirEntriesResponseDto>();

            var accessType = GetAccessType(path, username);
            if ((accessType & AccessType.Read) != 0)
            {
                List<EntryStruct> entries = _unitOfWork.BaseDirectoryRepository.GetEntries(path);
                foreach (var entry in entries)
                {
                    response.Add(new DirEntriesResponseDto
                    {
                        fileName = entry.fileName,
                        isDir = entry.isDir,
                        dateCreated = entry.dateCreated,
                        dateModified = entry.dateModified
                    });
                }
            }
            else
                throw new AccessDenied("no read priviledge");
            return response;

        }

        public void RenameDirOrFile(string path, string? username, string newName)
        {
            var accessType = GetAccessType(path, username);
            if ((accessType & AccessType.Write) == 0)
                throw new AccessDenied("no write priviledge");

            if (PathIsNotBaseDir(path) == false)
                throw new AccessDenied("base directory can not be modified from here");

            _unitOfWork.BaseDirectoryRepository.RenameDirOrFile(path, newName);
        }

        public void CreateDirectory(string path, string? username, string dirName)
        {
            var accessType = GetAccessType(path, username);
            if ((accessType & AccessType.Write) == 0)
                throw new AccessDenied("no write priviledge");
            if (PathIsNotUserDir(path) == false)
                throw new AccessDenied("base directory can not be created from here");

            _unitOfWork.BaseDirectoryRepository.CreateDirectory(path, dirName);
        }

        public void DeleteDirectoryOrFile(string path, string? username)
        {
            var accessType = GetAccessType(path, username);
            if ((accessType & AccessType.Write) == 0)
                throw new AccessDenied("no write priviledge");
            if (PathIsNotBaseDir(path) == false)
                throw new AccessDenied("base directory can not be deleted from here");

            _unitOfWork.BaseDirectoryRepository.DeleteDirectoryOrFile(path);
        }


        // Access modification

        public BaseDirectory? GetByDirNameAndAuthorFromPath(string path)
        {
            char separator = Path.DirectorySeparatorChar;
            string[] pathParts = Path.GetFullPath(path).Split(separator);
            if (pathParts.Length < 2)
            {
                throw new NoSuchFileOrDirectory("path too short");
            }

            string authorName = pathParts[0];
            string baseDirName = pathParts[1];

            User? user = _unitOfWork.UserRepository.FindByUserName(authorName);

            if (user == null)
                throw new NoSuchFileOrDirectory("invalid path");

            return _unitOfWork.BaseDirectoryRepository.GetByDirNameAndAuthor(baseDirName, user.Id);

        }
        public void ChangeAccessToBaseDir(string path, string? username, List<string> usernames, AccessType a, bool grantOrRewoke)
        {
            var accessType = GetAccessType(path, username);
            if ((accessType & AccessType.FullAccess) == 0)
                throw new AccessDenied("full access needed to change another user's access type");

            var baseDirToChangeAccess = GetByDirNameAndAuthorFromPath(path);
            if (baseDirToChangeAccess == null)
                throw new NoSuchFileOrDirectory("base directory not found from path");


            List<User> users = new List<User>();
            foreach(var usern in usernames)
            {
                User? user = _unitOfWork.UserRepository.FindByUserName(usern);

                if (user == null)
                    throw new BadUser(usern); // first user from the list that was not found
                else
                    users.Add(user);
            }

            foreach(var user in users)
            {
                _unitOfWork.BaseDirectoryRepository.ChangeAccessType(user.Id, baseDirToChangeAccess.Id, a, grantOrRewoke);
            }
        }


        public async Task<List<ClientSpaceUsageDto>> GetClientSpaceUsages(string? username)
        {
            var resp = new List<ClientSpaceUsageDto>();

            User? user = _unitOfWork.UserRepository.FindByUserName(username);
            if (user == null)
                throw new BadUser("not found by username");
            if (user.Role != Role.Admin)
                throw new BadUser("not an administrator");

            resp = await _unitOfWork.BaseDirectoryRepository.GetClientSpaceUsages();

            return resp;
        }

    }
}
