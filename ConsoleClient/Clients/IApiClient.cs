using ConsoleClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleClient.Clients
{
    public interface IApiClient
    {
        Task<IEnumerable<Student>> GetStudentsAsync();

        Task SubmitStudentAggregateAsync(IEnumerable<StudentAggregate> studentAggregates);
    }
}
