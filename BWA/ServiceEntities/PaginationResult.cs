namespace BWA.ServiceEntities
{
    public class PaginationResult<T> where T : class
    {
        public List<T> List { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
    }
}
