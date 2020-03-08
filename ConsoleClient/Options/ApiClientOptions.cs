namespace ConsoleClient.Options
{
    public class ApiClientOptions
    {
        public string BaseUrl { get; set; } = "http://apitest.sertifi.net/api";
        public string GetStudentsUri { get; set; } = "/Students";
        public string SubmitStudentAggregateUri { get; set; } = "/StudentAggregate";
    }
}
