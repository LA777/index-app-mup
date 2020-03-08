using ConsoleClient.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleClient.Clients
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IReadOnlyCollection<Student>> GetStudentsAsync()
        {
            var httpMethod = HttpMethod.Get;
            var requestUri = "http://apitest.sertifi.net/api/Students";// TODO LA - move URL to appsettings???
            var httpRequestMessage = new HttpRequestMessage(httpMethod, requestUri);
            _logger.LogInformation($"Request - Method: {httpMethod}; RequestURI: {requestUri}.");

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
            var contentType = httpResponseMessage.Content.Headers.ContentType.MediaType;
            _logger.LogInformation($"Response - StatusCode: {GetStatusCodeAsNumberAndString(httpResponseMessage.StatusCode)}; ContentType: {contentType}.");

            if (!httpResponseMessage.IsSuccessStatusCode)// TODO LA - check for application/json???
            {
                throw new Exception($"Request failed - StatusCode: {GetStatusCodeAsNumberAndString(httpResponseMessage.StatusCode)}");
            }

            if (string.IsNullOrEmpty(contentType) || contentType != MediaTypeNames.Application.Json)
            {
                throw new Exception($"Request failed - ContentType: {contentType}");
            }

            var contentJson = await httpResponseMessage.Content.ReadAsStringAsync();
            _logger.LogInformation($"Request content: {contentJson}.");
            var students = JsonSerializer.Deserialize<IReadOnlyCollection<Student>>(contentJson);

            return students;
        }

        public async Task SubmitStudentAggregateAsync(StudentAggregate studentAggregate)
        {
            var httpMethod = HttpMethod.Put;
            var requestUri = "http://apitest.sertifi.net/api/StudentAggregate";// TODO LA - move URL to appsettings???
            var httpRequestMessage = new HttpRequestMessage(httpMethod, requestUri);
            var json = JsonSerializer.Serialize(studentAggregate);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            _logger.LogInformation($"Request - Method: {httpMethod}; RequestURI: {requestUri}; Content {json}.");

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

            _logger.LogInformation($"Response - StatusCode: {GetStatusCodeAsNumberAndString(httpResponseMessage.StatusCode)}.");
            if (!httpResponseMessage.IsSuccessStatusCode)// TODO LA - check for application/json???
            {
                throw new Exception($"Request failed - StatusCode: {GetStatusCodeAsNumberAndString(httpResponseMessage.StatusCode)}");
            }
        }

        private string GetStatusCodeAsNumberAndString(HttpStatusCode statusCode)
        {
            return $"{(int)statusCode} {statusCode}";
        }
    }
}
