using ConsoleClient.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
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
            _logger.LogInformation($"Response - StatusCode: {httpResponseMessage.StatusCode}; ContentType: {contentType}.");

            if (!httpResponseMessage.IsSuccessStatusCode)// TODO LA - check for application/json???
            {
                throw new Exception($"Request failed - StatusCode: {httpResponseMessage.StatusCode}");
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

        // TODO LA - Cover with Unit Tests
        public async Task SubmitStudentAggregateAsync(StudentAggregate studentAggregates)
        {
            var httpMethod = HttpMethod.Put;
            var requestUri = "http://apitest.sertifi.net/api/StudentAggregate";// TODO LA - move URL to appsettings???
            var httpRequestMessage = new HttpRequestMessage(httpMethod, requestUri);
            _logger.LogInformation($"Request - Method: {httpMethod}; RequestURI: {requestUri}.");

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

            _logger.LogInformation($"Response - StatusCode: {httpResponseMessage.StatusCode}.");
            if (!httpResponseMessage.IsSuccessStatusCode)// TODO LA - check for application/json???
            {
                throw new Exception($"Request failed - StatusCode: {httpResponseMessage.StatusCode}");
            }
        }
    }
}
