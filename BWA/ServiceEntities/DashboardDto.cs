namespace BWA.ServiceEntities
{
    public class DashboardDto
    {
        public int Id { get; set; } = 0;
    }

    public class DashboardCountsDto
    {
        public int UsersCount { get; set; }
        public int BlogsCount { get; set; }
        public int TypesCount { get; set; }
    }

    public class DashboardBlogsDto
    {
        public int id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageExtension { get; set; } = string.Empty;
    }

    public class requestGraphDto
    {
        public DateTime Request { get; set; }
    }

    public class requestGraphCategoryDto : requestGraphDto
    {
        public int CategoryId { get; set; } = 0;
    }

    public class DashboardPostPerformanceDto
    {
        public int Id { get; set; }
        public string DataFor { get; set; } = string.Empty;
        public List<GraphData> Data { get; set; } = new List<GraphData>();
    }
    
    public class GraphData
    {
        public string Data { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
