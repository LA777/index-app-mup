using ConsoleClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleClient.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudents();
    }
}
