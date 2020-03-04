using AutoFixture;
using ConsoleClient.Clients;
using ConsoleClient.Models;
using FluentAssertions;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tests.Clients;
using Xunit;

namespace Tests
{
    public class ApiClientTests : TestBase
    {
        private ApiClient _sut;
        private readonly Mock<HttpClient> _httpClientMock;
        private Mock<FakeHttpMessageHandler> _fakeHttpMessageHandler;


        public ApiClientTests()
        {
            _fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            _httpClientMock = new Mock<HttpClient>(_fakeHttpMessageHandler.Object); // Set the FakeHttpMessageHandler as HttpClient's inner handler
            _sut = new ApiClient(_httpClientMock.Object);
        }

        [Fact]
        public async Task GetStudents_Should_Call_HttpClient_And_Return_Students_List()
        {
            // Arrange
            var students = Fixture.CreateMany<Student>();
            var studentsJson = JsonSerializer.Serialize(students);
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://apitest.sertifi.net/api/Students");
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(studentsJson),
                StatusCode = HttpStatusCode.OK
            };

            _fakeHttpMessageHandler.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<HttpCompletionOption>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _sut.GetStudentsAsync();

            // Assert
            result.Should().BeEquivalentTo(students);
            _fakeHttpMessageHandler.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<HttpCompletionOption>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
