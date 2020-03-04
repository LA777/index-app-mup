using AutoFixture;
using ConsoleClient.Clients;
using ConsoleClient.Models;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ApiClientTests : TestBase
    {
        private ApiClient _sut;
        private readonly Mock<HttpClient> _httpClientMock = new Mock<HttpClient>();


        public ApiClientTests()
        {
            _sut = new ApiClient(_httpClientMock.Object);
        }

        [Fact]
        public void GetStudents_Should_Call_HttpClient_And_Return_Students_List()
        {
            // Arrange
            var students = Fixture.CreateMany<Student>();
            var studentsJson = JsonSerializer.Serialize(students);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://apitest.sertifi.net/api/Students");
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(studentsJson),
                StatusCode = HttpStatusCode.OK
            };


            _httpClientMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>())
                .ReturnsAsync(httpResponseMessage);


            // Act
            var result = _sut.GetStudents();

            // Assert
            result.Should().BeEquivalentTo(students);
            _httpClientMock.Verify(x => x.SendAsync(httpRequestMessage), Times.Once);
            //_crmWebApiClientMock.Verify(m => m.GetDocumentByDocumentIdAsync(documentId));
        }
    }
}
