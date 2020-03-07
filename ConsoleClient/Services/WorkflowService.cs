﻿using ConsoleClient.Clients;
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
        private readonly IStudentService _studentService;

        public WorkflowService(IApiClientFactory apiClientFactory, ILogger<WorkflowService> logger, IStudentService studentService)
        {
            _apiClientFactory = apiClientFactory ?? throw new ArgumentNullException(nameof(apiClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
        }

        public async Task DoTestTaskAsync()
        {
            var apiClient = _apiClientFactory.Create();
            var students = await apiClient.GetStudentsAsync();

            var json = JsonSerializer.Serialize(students);
            _logger.LogDebug(json);


            //double a = 3.3;
            //double b = 4.8;
            //double c = a + b;

            //float a1 = 3.3f;
            //float b1 = 4.8f;
            //float c1 = a1 + b1;


            //var highestAttendanceYear = _studentService.GetHighestAttendanceYear(students);
            //var highestGPAYear = _studentService.GetHighestGPAYear(students);


            var ddd = _studentService.GetStudentIdMostInconsistent(students);
            //_logger.LogDebug($"highestAttendanceYear: {highestAttendanceYear}");

        }
    }
}
