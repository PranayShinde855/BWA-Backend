namespace BWA.APIInfrastructure.Requests
{
    public class CategoryRequests
    {
    }

    public class CategoryRequest
    {
        public int ID { get; set; }
        public string Category { get; set; }
    }

    public class GetCategoryDetailsRequest
    {
        public int ID { get; set; }
        public string Category { get; set; }
    }
    public class GetCategoryByIdRequest
    {
        public int ID { get; set; }
    }
    public class AddCategoryRequest
    {
        public string Category { get; set; }
    }

    public class UpdateCategoryRequest
    {
        public int ID { get; set; }
        public string Category { get; set; }
    }

    public class DeleteCategoryRequest
    {
        public int ID { get; set; }
    }
}
