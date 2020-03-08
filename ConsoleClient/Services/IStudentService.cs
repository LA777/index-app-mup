using ConsoleClient.Models;
using System.Collections.Generic;

namespace ConsoleClient.Services
{
    public interface IStudentService
    {
        int GetHighestAttendanceYear(IReadOnlyCollection<Student> students);

        int GetHighestGpaYear(IReadOnlyCollection<Student> students);

        IEnumerable<int> GetTopTenStudentsWithHighestGpa(IReadOnlyCollection<Student> students);

        int GetStudentIdMostInconsistent(IReadOnlyCollection<Student> students);
    }
}
