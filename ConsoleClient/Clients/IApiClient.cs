using ConsoleClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleClient.Clients
{
    public interface IApiClient
    {
        Task<IReadOnlyCollection<Student>> GetStudentsAsync();

        Task SubmitStudentAggregateAsync(IEnumerable<StudentAggregate> studentAggregates);
    }
}
