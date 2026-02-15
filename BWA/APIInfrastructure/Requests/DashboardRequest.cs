namespace BWA.APIInfrastructure.Requests
{
    public class DashboardRequest
    {
        public int Id { get; set; } = 0;
    }

    public class GetDashboardPostPerformanceDataRequest
    {
        public int Id { get; set; }
        public DateTime? Year { get; set; }
    }
}
