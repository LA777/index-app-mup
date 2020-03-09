using AutoFixture;
using ConsoleClient.Clients;
using ConsoleClient.Models;
using ConsoleClient.Options;
using ConsoleClient.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Services
{
    public class WorkflowServiceTests : TestBase
    {
        private readonly IWorkflowService _sut;
        private readonly Mock<IApiClientFactory> _apiClientFactoryMock;
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IStudentService> _studentServiceMock;
        private readonly string _name = "test1";
        private readonly string _email = "test1@test.com";

        public WorkflowServiceTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _apiClientFactoryMock = new Mock<IApiClientFactory>();
            var loggerMock = new Mock<ILogger<WorkflowService>>();
            _studentServiceMock = new Mock<IStudentService>();
            _apiClientFactoryMock.Setup(x => x.Create()).Returns(_apiClientMock.Object);
            var optionsMonitorMock = new Mock<IOptionsMonitor<UserDataOptions>>();
            var apiClientOptions = new UserDataOptions()
            {
                Name = _name,
                Email = _email
            };
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(apiClientOptions);

            _sut = new WorkflowService(_apiClientFactoryMock.Object, _studentServiceMock.Object, optionsMonitorMock.Object, loggerMock.Object);
        }

        [Fact]
        public void DoTestTaskAsync_Should_Call_ApiClient_And_StudentService()
        {
            // Arrange
            var students = Fixture.CreateMany<Student>().ToList();
            var studentAggregate = Fixture.Create<StudentAggregate>();
            studentAggregate.YourName = _name;
            studentAggregate.YourEmail = _email;
            StudentAggregate resultStudentAggregate = null;

            _apiClientMock.Setup(x => x.GetStudentsAsync()).ReturnsAsync(students);
            _studentServiceMock.Setup(x => x.GetHighestAttendanceYear(It.IsAny<IReadOnlyCollection<Student>>())).Returns(studentAggregate.YearWithHighestAttendance);
            _studentServiceMock.Setup(x => x.GetHighestGpaYear(It.IsAny<IReadOnlyCollection<Student>>())).Returns(studentAggregate.YearWithHighestOverallGpa);
            _studentServiceMock.Setup(x => x.GetTopTenStudentsWithHighestGpa(It.IsAny<IReadOnlyCollection<Student>>())).Returns(studentAggregate.Top10StudentIdsWithHighestGpa);
            _studentServiceMock.Setup(x => x.GetStudentIdMostInconsistent(It.IsAny<IReadOnlyCollection<Student>>())).Returns(studentAggregate.StudentIdMostInconsistent);
            _apiClientMock
                .Setup(x => x
                    .SubmitStudentAggregateAsync(It.IsAny<StudentAggregate>()))
                .Callback((StudentAggregate sa) => resultStudentAggregate = sa);

            // Act
            _sut.DoTestTaskAsync();

            // Assert
            _apiClientFactoryMock.Verify(x => x.Create(), Times.Once);
            _apiClientMock.Verify(x => x.GetStudentsAsync(), Times.Once);
            _studentServiceMock.Verify(x => x.GetHighestAttendanceYear(students), Times.Once);
            _studentServiceMock.Verify(x => x.GetHighestGpaYear(students), Times.Once);
            _studentServiceMock.Verify(x => x.GetTopTenStudentsWithHighestGpa(students), Times.Once);
            _studentServiceMock.Verify(x => x.GetStudentIdMostInconsistent(students), Times.Once);
            _apiClientMock.Verify(x => x.SubmitStudentAggregateAsync(It.IsAny<StudentAggregate>()), Times.Once);
            resultStudentAggregate.Should().BeEquivalentTo(studentAggregate);
        }
    }
}
