using ConsoleClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClient.Clients
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://apitest.sertifi.net/api/Students");
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)// TODO LA - check for application/json
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true
                };

                var students = await JsonSerializer.DeserializeAsync<IEnumerable<Student>>(contentStream, jsonSerializerOptions);

                return students;
            }
            else
            {
                throw new Exception($"Response status code: {httpResponseMessage.StatusCode}");
            }
        }

        public async Task SubmitStudentAggregate(IEnumerable<StudentAggregate> studentAggregates)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "http://apitest.sertifi.net/api/StudentAggregate");
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)// TODO LA - check for application/json
            {
                var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Response status code: {httpResponseMessage.StatusCode}");
            }
        }
    }
}
