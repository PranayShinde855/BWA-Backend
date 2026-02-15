namespace BWA.APIInfrastructure.Requests
{

    public class GetBlogsRequest : CommonPaginationProperties
    {       
    }

    public class AddBlogRequest
    {
        public int CategoryId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }
    }

    public class UpdateBlogRequest : GetBlogByIdRequest
    {
        public int CategoryId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }
    }

    public class GetBlogByIdRequest
    {
        public int Id { get; set; }
    }

    public class DeleteBlogRequest : GetBlogByIdRequest
    {
    }

    public class GetBlogByCategoryIdRequest
    {
        public int CategoryId { get; set; }
    }
}
