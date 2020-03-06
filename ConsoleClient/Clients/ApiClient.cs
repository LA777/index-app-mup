using ConsoleClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
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

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://apitest.sertifi.net/api/Students");
            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Response status code: {httpResponseMessage.StatusCode}");
            }

            await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var jsonSerializerOptions = new JsonSerializerOptions { AllowTrailingCommas = true };
            var students = await JsonSerializer.DeserializeAsync<IEnumerable<Student>>(contentStream, jsonSerializerOptions);

            return students;

        }

        public async Task SubmitStudentAggregateAsync(IEnumerable<StudentAggregate> studentAggregates)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "http://apitest.sertifi.net/api/StudentAggregate");
            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

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
