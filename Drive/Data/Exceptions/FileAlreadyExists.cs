namespace Drive.Data.Exceptions
{
    public class FileAlreadyExists: Exception
    {
        public FileAlreadyExists(string msg = ""): base(msg) { }
    }
}
