using ConsoleClient.Clients;
using System;
using System.Collections.Generic;

namespace ConsoleClient.Services
{
    public class StudentService : IStudentService
    {
        private readonly IApiClient _apiClient;

        public StudentService(IApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public int GetHighestAttendanceYear()
        {
            throw new NotImplementedException();
        }

        public int GetHighestGPAYear()
        {
            throw new NotImplementedException();
        }

        public int GetStudentIdMostInconsistent()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetTopStudentsWithHighestGPA()
        {
            throw new NotImplementedException();
        }
    }
}
