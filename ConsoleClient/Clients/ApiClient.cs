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
        private readonly IHttpClientFactory _clientFactory;

        public ApiClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://apitest.sertifi.net/api/Students");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)// TODO LA - check for application/json
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true
                };

                var students = JsonSerializer.Deserialize<IEnumerable<Student>>(responseContent, options);

                return students;
            }
            else
            {
                throw new Exception($"Response status code: {response.StatusCode}");
            }
        }

        public async Task SubmitStudentAggregate(IEnumerable<StudentAggregate> studentAggregates)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "http://apitest.sertifi.net/api/StudentAggregate");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)// TODO LA - check for application/json
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Response status code: {response.StatusCode}");
            }
        }
    }
}
