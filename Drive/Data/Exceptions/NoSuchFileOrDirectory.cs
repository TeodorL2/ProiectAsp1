namespace Drive.Data.Exceptions
{
    public class NoSuchFileOrDirectory: Exception
    {
        public NoSuchFileOrDirectory(string msg = "") : base(msg) { }
    }
}
