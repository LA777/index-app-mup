namespace ConsoleClient.Options
{
    public class ApiClientOptions
    {
        public string BaseUrl { get; set; } = "http://apitest.sertifi.net";
        public string GetStudentsUri { get; set; } = "/api/Students";
        public string SubmitStudentAggregateUri { get; set; } = "/api/StudentAggregate";
    }
}
