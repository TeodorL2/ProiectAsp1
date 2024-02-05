using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Drive2.StorageManagement
{
    public class StorageManagement
    {
        private readonly string _baseDir;
        public StorageManagement(string baseDir) {
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

            if(Directory.Exists(fullPath))
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
                }
            }

            return rez;
        }


        public async Task<bool> UploadFiles(string path, List<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    // Get the stream of the file
                    using (var stream = file.OpenReadStream())
                    {
                        // Specify the folder path on your server
                        var filePath = Path.Combine(path, file.FileName);

                        // Save the file to the server
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateDir(string path, string dirName)
        {
            string newPath = Path.Combine(_baseDir, path);
            // Check if the base path is valid
            if (!Directory.Exists(newPath))
            {
                return false;
            }

            // Combine the base path and new directory name to create the full path
            newPath = Path.Combine(newPath, dirName);

            // Check if another directory with the same name already exists
            if (Directory.Exists(newPath))
            {
                return false;
            }

            // Create the new directory
            Directory.CreateDirectory(newPath);
            return true;
        }

        public bool Delete(string path)
        {
            string newPath = Path.Combine(_baseDir, path);
            // Check if the path exists
            if (Directory.Exists(newPath))
            {
                // Delete the directory if it exists
                Directory.Delete(newPath, true);
                return true;
            }
            else if (File.Exists(newPath))
            {
                // Delete the file if it exists
                File.Delete(newPath);
                return true;
            }
            return false;
        }

        public bool Rename(string path, string newName)
        {
            string newPath = Path.Combine(_baseDir, path);
            // Check if the path exists
            if (Directory.Exists(newPath))
            {
                // Rename the directory if it exists
                string newPathRenamed = Path.Combine(Path.GetDirectoryName(newPath), newName);
                Directory.Move(newPath, newPathRenamed);
                return true;
            }
            else if (File.Exists(newPath))
            {
                // Rename the file if it exists
                string newPathRenamed = Path.Combine(Path.GetDirectoryName(newPath), newName);
                File.Move(newPath, newPathRenamed);
                return true;
            }
            return false;
        }
    }
}
