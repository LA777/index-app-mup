using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleClient.Services
{
    public interface IStudentService
    {
        Task<int> GetHighestAttendanceYear();

        Task<int> GetHighestGPAYear();

        Task<IEnumerable<int>> GetTopStudentsWithHighestGPA();

        Task<int> GetStudentIdMostInconsistent();
    }
}
