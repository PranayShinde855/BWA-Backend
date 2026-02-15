namespace BWA.APIInfrastructure.Requests
{
    public class CommonPaginationProperties
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public string GlobalSearch { get; set; }
    }
}
