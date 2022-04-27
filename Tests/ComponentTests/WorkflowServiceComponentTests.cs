using ConsoleClient.Clients;
using ConsoleClient.Models;
using ConsoleClient.Options;
using ConsoleClient.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tests.ComponentTests
{
    public class WorkflowServiceComponentTests : TestBase
    {
        private readonly IWorkflowService _sut;
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly string _name = "test1";
        private readonly string _email = "test1@test.com";

        public WorkflowServiceComponentTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            var apiClientFactoryMock = new Mock<IApiClientFactory>();
            var loggerMock = new Mock<ILogger<WorkflowService>>();
            var studentService = new StudentService();
            apiClientFactoryMock.Setup(x => x.Create()).Returns(_apiClientMock.Object);
            var optionsMonitorMock = new Mock<IOptionsMonitor<UserDataOptions>>();
            var apiClientOptions = new UserDataOptions()
            {
                Name = _name,
                Email = _email
            };
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(apiClientOptions);

            _sut = new WorkflowService(apiClientFactoryMock.Object, studentService, optionsMonitorMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task DoTestTaskAsync_Should_Call_ApiClient_And_StudentService_And_Submit_Correct_Values()
        {
            // Arrange
            var students = GetRealData();
            var expectedStudentAggregate = new StudentAggregate()
            {
                YourName = _name,
                YourEmail = _email,
                StudentIdMostInconsistent = 15,
                YearWithHighestAttendance = 2011,
                YearWithHighestOverallGpa = 2013,
                Top10StudentIdsWithHighestGpa = new[] { 18, 4, 12, 11, 20, 13, 24, 6, 22, 21 }
            };
            StudentAggregate resultStudentAggregate = null;

            _apiClientMock.Setup(x => x.GetStudentsAsync()).ReturnsAsync(students);
            _apiClientMock
                .Setup(x => x
                    .SubmitStudentAggregateAsync(It.IsAny<StudentAggregate>()))
                .Callback((StudentAggregate sa) => resultStudentAggregate = sa);

            // Act
            await _sut.DoTestTaskAsync();

            // Assert
            resultStudentAggregate.Should().BeEquivalentTo(expectedStudentAggregate);
        }

        private static IReadOnlyCollection<Student> GetRealData()
        {
            const string fileNameInDirectory = @"ComponentTests\students.json";
            var currentDirectory = Directory.GetCurrentDirectory();
            var fileFullPath = $@"{currentDirectory}\{fileNameInDirectory}";
            var json = File.ReadAllText(fileFullPath);
            var students = JsonSerializer.Deserialize<IReadOnlyCollection<Student>>(json);

            return students;
        }
    }
}
