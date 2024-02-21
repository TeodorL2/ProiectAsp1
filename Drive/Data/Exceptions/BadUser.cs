namespace Drive.Data.Exceptions
{
    public class BadUser: Exception
    {
        public BadUser(string msg = "") : base(msg) { }
    }
}
