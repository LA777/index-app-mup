using ConsoleClient.Clients;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleClient.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IApiClientFactory _apiClientFactory;
        private readonly ILogger _logger;

        public WorkflowService(IApiClientFactory apiClientFactory, ILogger logger)
        {
            _apiClientFactory = apiClientFactory ?? throw new ArgumentNullException(nameof(apiClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DoTestTaskAsync()
        {
            var apiClient = _apiClientFactory.Create();
            var students = await apiClient.GetStudentsAsync();
            var json = JsonSerializer.Serialize(students);
            _logger.LogDebug(json);




        }
    }
}
