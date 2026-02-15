namespace BWA.Utility.Exception
{
    public class PermissionResultException : System.Exception
    {
        public PermissionResultException()
        {
        }
        public PermissionResultException(string message) : base(message)
        {
        }
        public PermissionResultException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
