namespace BWA.Utility.Exception
{
    public class DuplicateRecordException : System.Exception
    {
        public DuplicateRecordException()
        {
        }
        public DuplicateRecordException(string message) : base(message)
        {
        }
        public DuplicateRecordException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
