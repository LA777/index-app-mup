using ConsoleClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.Services
{
    public class StudentService : IStudentService
    {
        public int? GetHighestAttendanceYear(IReadOnlyCollection<Student> students)
        {
            if (students == null || !students.Any())
            {
                return null;
            }

            var attendanceYears = new List<int>();

            foreach (var student in students)
            {
                attendanceYears.AddRange(GetRangeValues(student.StartYear, student.EndYear));
            }

            var grouped = attendanceYears.GroupBy(x => x);
            var yearsWithCount = grouped.Select(x => (x.Key, x.Count()));
            var ordered = yearsWithCount.OrderByDescending(x => x.Item2).ThenBy(x => x.Key);
            var highestAttendanceYear = ordered.First().Key;

            return highestAttendanceYear;
        }

        private IEnumerable<int> GetRangeValues(int start, int end)
        {
            if (start > end)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Start value is bigger than end value");
            }

            for (int i = start; i <= end; i++)
            {
                yield return i;
            }
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
