using ConsoleClient.Clients;
using ConsoleClient.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ConsoleClient.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IApiClientFactory _apiClientFactory;
        private readonly ILogger<WorkflowService> _logger;
        private readonly IStudentService _studentService;

        public WorkflowService(IApiClientFactory apiClientFactory, IStudentService studentService, ILogger<WorkflowService> logger)
        {
            _apiClientFactory = apiClientFactory ?? throw new ArgumentNullException(nameof(apiClientFactory));
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DoTestTaskAsync()
        {
            var apiClient = _apiClientFactory.Create();
            var students = await apiClient.GetStudentsAsync();

            var highestAttendanceYear = _studentService.GetHighestAttendanceYear(students);
            var highestGpaYear = _studentService.GetHighestGPAYear(students);
            var topTenStudentsWithHighestGpa = _studentService.GetTopTenStudentsWithHighestGPA(students);
            var studentIdMostInconsistent = _studentService.GetStudentIdMostInconsistent(students);

            var studentAggregate = new StudentAggregate()
            {
                YourName = "test1", // TODO LA - take data from appsettings
                YourEmail = "test1@test.com",
                StudentIdMostInconsistent = studentIdMostInconsistent,
                Top10StudentIdsWithHighestGpa = topTenStudentsWithHighestGpa,
                YearWithHighestAttendance = highestAttendanceYear,
                YearWithHighestOverallGpa = highestGpaYear
            };

            _logger.LogInformation($"studentAggregate: {studentAggregate}");

            await apiClient.SubmitStudentAggregateAsync(studentAggregate);
        }
    }
}
