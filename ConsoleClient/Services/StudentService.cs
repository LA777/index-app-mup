using ConsoleClient.Clients;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleClient.Services
{
    public class StudentService : IStudentService
    {
        private readonly IApiClient _apiClient;

        public StudentService(IApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public async Task<int> GetHighestAttendanceYear()
        {

            throw new NotImplementedException();
        }

        public Task<int> GetHighestGPAYear()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetStudentIdMostInconsistent()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> GetTopStudentsWithHighestGPA()
        {
            throw new NotImplementedException();
        }
    }
}
