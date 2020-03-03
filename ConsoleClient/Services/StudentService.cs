using ConsoleClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleClient.Services
{
    public class StudentService : IStudentService
    {
        private readonly IHttpClientFactory _clientFactory;

        public StudentService(IHttpClientFactory clientFactory)
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
    }
}
