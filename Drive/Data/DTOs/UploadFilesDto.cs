namespace Drive.Data.DTOs
{
    public class UploadFilesDto
    {
        public string Path {  get; set; }

        public List<IFormFile> Files { get; set; }
    }
}
