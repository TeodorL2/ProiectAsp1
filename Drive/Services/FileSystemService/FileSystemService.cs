using Drive.Data.Exceptions;
using Drive.StorageManagement.StorageManagement;
using Drive.StorageManagement.StorageManagement.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http.Formatting;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drive.Services.FileSystemService
{
    public class FileSystemService: IFileSystemService
    {
        public readonly string urlToAsk = "https://localhost:7180/api/FileSystem/";

        private readonly HttpClient _httpClient;

        public FileSystemService(/* HttpClient httpClient */)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            // _httpClient = httpClient;
            _httpClient = new HttpClient(handler);
        }

        public async Task<List<EntryStruct>> GetEntries(string path)
        {
            var resp = await _httpClient.PostAsJsonAsync(urlToAsk + "get-entries", path);
            var content = await resp.Content.ReadAsStringAsync();

            if (resp.IsSuccessStatusCode)
            {
                var rez = JsonSerializer.Deserialize<List<EntryStruct>>(content);
                if (rez == null)
                    return new List<EntryStruct>();
                else 
                    return rez;
            }
            else
                if(resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new NoSuchFileOrDirectory(content);
            }
            else
                throw new Exception(resp.StatusCode.ToString());
        }

        public async Task CreateDir(string path, string dirName)
        {
            var resp = await _httpClient.PostAsJsonAsync(urlToAsk + "create-dir", new CreateDirDto { Path = path, DirName = dirName});
            var content = await resp.Content.ReadAsStringAsync();
            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new NoSuchFileOrDirectory(content);
            else if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new FileAlreadyExists(content);
        }

        public async Task Delete(string path)
        {
            var resp = await _httpClient.PostAsJsonAsync(urlToAsk + "delete-item", path);
            var content = await resp.Content.ReadAsStringAsync();

            if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new Exception(content);
        }

        public async Task UploadFiles(string path, List<IFormFile> files)
        {
            var obj = new ObjectContent<UploadFilesDto>(new UploadFilesDto { Path= path, Files = files}, new JsonMediaTypeFormatter());
            var resp = await _httpClient.PostAsync(urlToAsk + "upload-files", obj);
            var content = await resp.Content.ReadAsStringAsync();

            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new NoSuchFileOrDirectory(content);
            else if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new Exception(content);
        }

        public async Task<Stream> DownloadDirOrFile(string path)
        {
            var resp = await _httpClient.PostAsJsonAsync(urlToAsk + "download-items", path);
            var content = await resp.Content.ReadAsStringAsync();

            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var str = await resp.Content.ReadAsStreamAsync();
                return str;
            }

            if(resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new NoSuchFileOrDirectory(content);
            else if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new Exception(content);

            throw new Exception(content);
        }

        public async Task Rename(string path, string newName)
        {
            var resp = await _httpClient.PostAsJsonAsync(urlToAsk + "rename-item", new RenameItemDto { Path = path, NewName = newName});
            var content = await resp.Content.ReadAsStringAsync();

            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new NoSuchFileOrDirectory(content);
            else if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new FileAlreadyExists(content);
            else if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new Exception(content);
        }
    }
}
