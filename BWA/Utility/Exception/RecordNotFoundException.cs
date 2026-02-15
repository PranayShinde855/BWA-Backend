namespace BWA.Utility.Exception
{
    public class RecordNotFoundException : System.Exception
    {
        public RecordNotFoundException()
        {
        }
        public RecordNotFoundException(string message) : base(message)
        {
        }
        public RecordNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
