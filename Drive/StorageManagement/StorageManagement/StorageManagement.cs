using Drive.Data.Exceptions;
using System.IO.Compression;

namespace Drive.StorageManagement.StorageManagement
{
    public class StorageManagement
    {
        private readonly string _baseDir;

        public StorageManagement(string baseDir)
        {
            _baseDir = baseDir;
        }

        bool CheckPath(string path)
        {
            string fullPath = Path.Combine(_baseDir, path);
            return File.Exists(fullPath) || Directory.Exists(fullPath);
        }

        public List<EntryStruct> GetEntries(string path)
        {
            string fullPath = Path.Combine(_baseDir, path);
            List<EntryStruct> rez = new List<EntryStruct>();

            if (Directory.Exists(fullPath))
            {
                try
                {
                    foreach (string i in Directory.GetDirectories(fullPath))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(fullPath, i));
                        rez.Add(new EntryStruct(directoryInfo.Name, true, directoryInfo.CreationTime, directoryInfo.LastWriteTime));
                    }

                    foreach (string i in Directory.GetFiles(fullPath))
                    {
                        FileInfo fileInfo = new FileInfo(Path.Combine(fullPath, i));
                        rez.Add(new EntryStruct(fileInfo.Name, false, fileInfo.CreationTime, fileInfo.LastWriteTime));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error EntriesRec " + ex.Message);
                    throw new NoSuchFileOrDirectory("error while getting files info");
                }
            }
            else
            {
                throw new NoSuchFileOrDirectory("path not found");
            }

            return rez;
        }


        public async Task UploadFiles(string path, List<IFormFile> files)
        {
            path = Path.Combine(_baseDir, path);

            if (!Directory.Exists(path)) 
            { 
                throw new NoSuchFileOrDirectory("path not found"); 
            }

            try
            {
                foreach (var file in files)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var filePath = Path.Combine(path, file.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new NoSuchFileOrDirectory("error during upload"); // can be another exception, more specific
            }
        }

        public async Task<Stream> DownloadDirOrFile(string path)
        {
            path = Path.Combine (_baseDir, path);

            var file = Path.GetFileName(path);

            if (Directory.Exists(path))
            {
                var tempZipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), $"{file}.zip");

                ZipFile.CreateFromDirectory(path, tempZipPath, CompressionLevel.Optimal, false);

                var fileStream = new FileStream(tempZipPath, FileMode.Open, FileAccess.Read);

                // fileStream.OnDispose(() => File.Delete(tempZipPath));

                return fileStream;
            }
            else
                if (File.Exists(path))
            {
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

                // fileStream.;

                return fileStream;
            }
            else
                throw new NoSuchFileOrDirectory("path not found");
        }

        public void CreateDir(string path, string dirName)
        {
            string newPath = Path.Combine(_baseDir, path);
            if (!Directory.Exists(newPath))
            {
                throw new NoSuchFileOrDirectory("path not found");
            }

            newPath = Path.Combine(newPath, dirName);

            if (Directory.Exists(newPath))
            {
                throw new FileAlreadyExists("directory already exists");
            }

            Directory.CreateDirectory(newPath);
        }

        public void Delete(string path)
        {
            string newPath = Path.Combine(_baseDir, path);
            if (Directory.Exists(newPath))
            {
                Directory.Delete(newPath, true);
                return;
            }
            else if (File.Exists(newPath))
            {
                File.Delete(newPath);
                return;
            }
            // throw new NoSuchFileOrDirectory("path not found");
        }

        public void Rename(string path, string newName) // path must not end with separator
        {
            string newPath = Path.Combine(_baseDir, path);

            string oldName = Path.GetFileName(newPath);
            if (oldName == newName)
                return;

            string? parentDir = Path.GetDirectoryName(newPath);
            if (parentDir == null)
                throw new NoSuchFileOrDirectory("root directory or null path");

            string newPathRenamed = Path.Combine(parentDir, newName);
            if (Directory.Exists(newPathRenamed) || File.Exists(newPathRenamed))
                throw new FileAlreadyExists("rename attempt to an already existing file");
            
            if (Directory.Exists(newPath))
            {
                Directory.Move(newPath, newPathRenamed);
                return;
            }
            else if (File.Exists(newPath))
            {
                File.Move(newPath, newPathRenamed);
                return;
            }
            throw new NoSuchFileOrDirectory("path not found");
        }
    }
}
