using AutoFixture;
using ConsoleClient.Clients;
using ConsoleClient.Models;
using ConsoleClient.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Clients
{
    public class ApiClientTests : TestBase
    {
        private readonly ApiClient _sut;
        private readonly Mock<FakeHttpMessageHandler> _fakeHttpMessageHandler;

        public ApiClientTests()
        {
            const string baseUri = "http://apitest.sertifi.net";
            _fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            var httpClient = new HttpClient(_fakeHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(baseUri)
            };
            var optionsMonitorMock = new Mock<IOptionsMonitor<ApiClientOptions>>();
            var apiClientOptions = new ApiClientOptions()
            {
                BaseUrl = baseUri,
                GetStudentsUri = "api/Students",
                SubmitStudentAggregateUri = "api/StudentAggregate"
            };
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(apiClientOptions);
            var loggerMock = new Mock<ILogger<ApiClient>>();

            _sut = new ApiClient(httpClient, optionsMonitorMock.Object, loggerMock.Object);
        }

        #region GetStudentsAsyncTests

        [Fact]
        public async Task GetStudentsAsync_Should_Call_HttpClient_And_Return_Students_List()
        {
            // Arrange
            var students = Fixture.CreateMany<Student>().ToList();
            var studentsJson = JsonSerializer.Serialize(students);
            var httpRequestMessage = (HttpRequestMessage)null;
            var expectedMethod = HttpMethod.Get;
            var expectedUrl = "http://apitest.sertifi.net/api/Students";

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(studentsJson, Encoding.UTF8, MediaTypeNames.Application.Json),
                StatusCode = HttpStatusCode.OK
            };

            _fakeHttpMessageHandler.
                Setup(x => x.
                    SendAsync(It.IsAny<HttpRequestMessage>()))
                .Callback((HttpRequestMessage m) => httpRequestMessage = m)
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _sut.GetStudentsAsync();

            // Assert
            result.Should().BeEquivalentTo(students);
            _fakeHttpMessageHandler.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
            httpRequestMessage.Should().NotBeNull();
            httpRequestMessage.Method.Should().Be(expectedMethod);
            httpRequestMessage.RequestUri.Should().Be(expectedUrl);
        }

        [Fact]
        public async Task GetStudentsAsync_Should_Throw_Exception_If_Response_StatusCode_Is_Not_Success()
        {
            // Arrange
            var students = Fixture.CreateMany<Student>().ToList();
            var studentsJson = JsonSerializer.Serialize(students);

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(studentsJson),
                StatusCode = HttpStatusCode.Unauthorized
            };

            _fakeHttpMessageHandler.
                Setup(x => x.
                    SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponseMessage);

            // Act and assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _sut.GetStudentsAsync();
            });
        }

        [Fact]
        public async Task GetStudentsAsync_Should_Throw_Exception_If_Response_ContentType_Is_Not_ApplicationJson()
        {
            // Arrange
            var students = Fixture.CreateMany<Student>().ToList();
            var studentsJson = JsonSerializer.Serialize(students);

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(studentsJson, Encoding.UTF8, MediaTypeNames.Application.Xml),
                StatusCode = HttpStatusCode.OK
            };

            _fakeHttpMessageHandler.
                Setup(x => x.
                    SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponseMessage);

            // Act and assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _sut.GetStudentsAsync();
            });
        }

        [Fact]
        public async Task GetStudentsAsync_Should_Throw_Exception_If_Response_ContentType_Is_Null()
        {
            // Arrange
            var students = Fixture.CreateMany<Student>().ToList();
            var studentsJson = JsonSerializer.Serialize(students);

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(studentsJson, Encoding.UTF8, null),
                StatusCode = HttpStatusCode.OK
            };

            _fakeHttpMessageHandler.
                Setup(x => x.
                    SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponseMessage);

            // Act and assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _sut.GetStudentsAsync();
            });
        }

        #endregion

        #region SubmitStudentAggregateAsync

        [Fact]
        public async Task SubmitStudentAggregateAsync_Should_Call_HttpClient_And_Submit_Data()
        {
            // Arrange
            var studentAggregate = Fixture.Create<StudentAggregate>();
            var httpRequestMessage = (HttpRequestMessage)null;
            var expectedMethod = HttpMethod.Put;
            var expectedUrl = "http://apitest.sertifi.net/api/StudentAggregate";
            var json = JsonSerializer.Serialize(studentAggregate);
            var expectedContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent
            };

            _fakeHttpMessageHandler.
                Setup(x => x.
                    SendAsync(It.IsAny<HttpRequestMessage>()))
                .Callback((HttpRequestMessage m) => httpRequestMessage = m)
                .ReturnsAsync(httpResponseMessage);

            // Act
            await _sut.SubmitStudentAggregateAsync(studentAggregate);

            // Assert
            _fakeHttpMessageHandler.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
            httpRequestMessage.Should().NotBeNull();
            httpRequestMessage.Method.Should().Be(expectedMethod);
            httpRequestMessage.RequestUri.Should().Be(expectedUrl);
            httpRequestMessage.Content.Should().BeEquivalentTo(expectedContent);
        }

        [Fact]
        public async Task SubmitStudentAggregateAsync_Should_Throw_Exception_If_Response_StatusCode_Is_Not_Success()
        {
            // Arrange
            var studentAggregate = Fixture.Create<StudentAggregate>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NetworkAuthenticationRequired
            };

            _fakeHttpMessageHandler.
                Setup(x => x.
                    SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponseMessage);

            // Act and assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _sut.SubmitStudentAggregateAsync(studentAggregate);
            });
        }

        #endregion
    }
}
