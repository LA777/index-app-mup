using ConsoleClient.Models;
using System.Collections.Generic;

namespace ConsoleClient.Services
{
    public interface IStudentService
    {
        int? GetHighestAttendanceYear(IReadOnlyCollection<Student> students);

        int GetHighestGPAYear();

        IEnumerable<int> GetTopStudentsWithHighestGPA();

        int GetStudentIdMostInconsistent();
    }
}
