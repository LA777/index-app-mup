using ConsoleClient.Models;
using ConsoleClient.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly ApiClientOptions _options;

        public ApiClient(HttpClient httpClient, IOptionsMonitor<ApiClientOptions> optionsAccessor, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            var accessor = optionsAccessor ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _options = accessor.CurrentValue ?? throw new ArgumentNullException(nameof(accessor.CurrentValue));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IReadOnlyCollection<Student>> GetStudentsAsync()
        {
            var httpMethod = HttpMethod.Get;
            var httpRequestMessage = new HttpRequestMessage(httpMethod, _options.GetStudentsUri);
            _logger.LogInformation($"Request - Method: {httpMethod}; RequestURI: {_httpClient.BaseAddress.OriginalString}/{_options.GetStudentsUri}.");

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
            var contentType = httpResponseMessage.Content.Headers.ContentType.MediaType;
            _logger.LogInformation($"Response - StatusCode: {GetStatusCodeAsNumberAndString(httpResponseMessage.StatusCode)}; ContentType: {contentType}.");

            if (!httpResponseMessage.IsSuccessStatusCode)
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
            var httpRequestMessage = new HttpRequestMessage(httpMethod, _options.SubmitStudentAggregateUri);
            var json = JsonSerializer.Serialize(studentAggregate);
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            _logger.LogInformation($"Request - Method: {httpMethod}; RequestURI: {_httpClient.BaseAddress.OriginalString}/{_options.SubmitStudentAggregateUri}; Content {json}.");

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

            _logger.LogInformation($"Response - StatusCode: {GetStatusCodeAsNumberAndString(httpResponseMessage.StatusCode)}.");
            if (!httpResponseMessage.IsSuccessStatusCode)
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
