using BWA.APIInfrastructure.Requests;

namespace BWA.ServiceEntities
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }


    public class GetBlogsDto : CommonPaginationProperties
    {
        public int ActionBy { get; set; }
        public string Role { get; set; }
    }

    public class GetBlogsDetailsDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageExtension { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }

    public class GetUserBlogsDetailsDto : GetBlogsDetailsDto
    {
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class AddBlogDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageExtension { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int ActionBy { get; set; }
    }

    public class UpdateBlogDto: AddBlogDto
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
    }

    public class GetBlogByIdDto
    {
        public int Id { get; set; }
    }

    public class DeleteBlogDto
    {
        public int Id { get; set; }
        public int ActionBy { get; set; }
    }

    public class GetBlogByCategoryIdDto
    {
        public int CategoryId { get; set; }
    }
}
