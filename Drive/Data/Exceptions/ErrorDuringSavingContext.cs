namespace Drive.Data.Exceptions
{
    public class ErrorDuringSavingContext: Exception
    {
        public ErrorDuringSavingContext(string msg=""): base (msg) { }
    }
}
