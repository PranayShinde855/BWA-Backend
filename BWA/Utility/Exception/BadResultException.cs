namespace BWA.Utility.Exception
{
    public class BadResultException : System.Exception
    {
        public BadResultException()
        {
        }
        public BadResultException(string message) : base(message)
        {
        }
        public BadResultException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
