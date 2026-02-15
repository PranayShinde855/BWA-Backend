namespace BWA.ServiceEntities
{
    public class CategoryDto
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
    }

    public class GetCategoryDetailsDto
    {
        public int ID { get; set; }
        public string Category { get; set; }
    }
    public class GetCategoryByIdDto
    {
        public int ID { get; set; }
    }
    public class AddCategoryDto
    {
        public string Category { get; set; }
    }

    public class UpdateCategoryDto
    {
        public int ID { get; set; }
        public string Category { get; set; }
    }

    public class DeleteCategoryDto
    {
        public int ID { get; set; }
    }
}
