namespace Drive.Data.Exceptions
{
    public class AccessDenied: Exception
    {
        public AccessDenied(string msg = ""): base(msg) { }
    }
}
