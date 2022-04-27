using ConsoleClient.Clients;
using ConsoleClient.Models;
using ConsoleClient.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ConsoleClient.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IApiClientFactory _apiClientFactory;
        private readonly ILogger<WorkflowService> _logger;
        private readonly IStudentService _studentService;
        private readonly UserDataOptions _options;

        public WorkflowService(IApiClientFactory apiClientFactory, IStudentService studentService, IOptionsMonitor<UserDataOptions> optionsAccessor, ILogger<WorkflowService> logger)
        {
            _apiClientFactory = apiClientFactory ?? throw new ArgumentNullException(nameof(apiClientFactory));
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            var accessor = optionsAccessor ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _options = accessor.CurrentValue ?? throw new ArgumentNullException(nameof(accessor.CurrentValue));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DoTestTaskAsync()
        {
            var apiClient = _apiClientFactory.Create();
            var students = await apiClient.GetStudentsAsync();

            var highestAttendanceYear = _studentService.GetHighestAttendanceYear(students);
            var highestGpaYear = _studentService.GetHighestGpaYear(students);
            var topTenStudentsWithHighestGpa = _studentService.GetTopTenStudentsWithHighestGpa(students);
            var studentIdMostInconsistent = _studentService.GetStudentIdMostInconsistent(students);

            var studentAggregate = new StudentAggregate()
            {
                YourName = _options.Name,
                YourEmail = _options.Email,
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
