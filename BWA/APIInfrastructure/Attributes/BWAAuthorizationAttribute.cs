namespace BWA.APIInfrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class BWAAuthorizationAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public string Name { get; }
        public BWAAuthorizationAttribute(string name = null) : base("Permission")
        {
            Name = name;
        }
    }
}

