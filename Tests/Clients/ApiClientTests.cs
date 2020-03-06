using AutoFixture;
using ConsoleClient.Clients;
using ConsoleClient.Models;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            _fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            var httpClientMock = new Mock<HttpClient>(_fakeHttpMessageHandler.Object);
            _sut = new ApiClient(httpClientMock.Object);
        }

        [Fact]
        public async Task GetStudents_Should_Call_HttpClient_And_Return_Students_List()
        {
            // Arrange
            var students = Fixture.CreateMany<Student>().ToList();
            var studentsJson = JsonSerializer.Serialize(students);
            var httpRequestMessage = (HttpRequestMessage)null;
            var expectedMethod = HttpMethod.Get;
            var expectedUrl = "http://apitest.sertifi.net/api/Students";

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(studentsJson),
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
            httpRequestMessage?.Method.Should().Be(expectedMethod);
            httpRequestMessage?.RequestUri.Should().Be(expectedUrl);
        }

        [Fact]
        public async Task GetStudents_Should_Throw_Exception_If_Response_StatusCode_Is_Not_Success()
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
    }
}
