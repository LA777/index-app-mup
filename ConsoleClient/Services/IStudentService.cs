using System.Collections.Generic;

namespace ConsoleClient.Services
{
    public interface IStudentService
    {
        int GetHighestAttendanceYear();

        int GetHighestGPAYear();

        IEnumerable<int> GetTopStudentsWithHighestGPA();

        int GetStudentIdMostInconsistent();
    }
}
