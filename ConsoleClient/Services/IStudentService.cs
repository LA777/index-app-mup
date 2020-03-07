using ConsoleClient.Models;
using System.Collections.Generic;

namespace ConsoleClient.Services
{
    public interface IStudentService
    {
        int GetHighestAttendanceYear(IReadOnlyCollection<Student> students);

        int GetHighestGPAYear(IReadOnlyCollection<Student> students);

        IEnumerable<int> GetTopStudentsWithHighestGPA();

        int GetStudentIdMostInconsistent();
    }
}
