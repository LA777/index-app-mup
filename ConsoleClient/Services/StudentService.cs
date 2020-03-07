using ConsoleClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.Services
{
    public class StudentService : IStudentService
    {
        /// <summary>
        /// Return the year which saw the highest attendance – if there are ties,
        /// display the earliest year
        /// </summary>
        /// <param name="students"></param>
        /// <returns>Year</returns>
        public int GetHighestAttendanceYear(IReadOnlyCollection<Student> students)
        {
            if (students == null)
            {
                throw new ArgumentNullException(nameof(students));
            }

            if (!students.Any())
            {
                throw new ArgumentException("Collection is empty.", nameof(students));
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

        /// <summary>
        /// Returns the year with highest overall GPA
        /// </summary>
        /// <param name="students"></param>
        /// <returns>Year</returns>
        public int GetHighestGPAYear(IReadOnlyCollection<Student> students)
        {
            var dict = new Dictionary<int, decimal>();

            foreach (var student in students)
            {
                var yearsRange = GetRangeValues(student.StartYear, student.EndYear).ToList();
                var gpaResords = student.GPARecord.ToList();
                if (yearsRange.Count() != gpaResords.Count())
                {
                    throw new ArgumentOutOfRangeException();// TODO LA - Create custom exception
                }

                for (int i = 0; i < gpaResords.Count; i++)
                {
                    var year = yearsRange[i];
                    decimal gpa = gpaResords[i];
                    decimal val = dict.GetValueOrDefault(year);
                    decimal sum = val + gpa;
                    dict[year] = sum;
                }
            }

            var ordered = dict.OrderByDescending(x => x.Value);
            var highestGPAYear = ordered.First().Key;

            return highestGPAYear;
        }

        /// <summary>
        /// Returns IDs of the top 10 students with highest overall GPA
        /// </summary>
        /// <returns>List of student's IDs</returns>
        public IEnumerable<int> GetTopStudentsWithHighestGPA(IReadOnlyCollection<Student> students)
        {
            var dict = new Dictionary<int, decimal>();

            foreach (var student in students)
            {
                dict[student.Id] = student.GPARecord.Sum();
            }

            var tops = dict.OrderByDescending(x => x.Value).Take(10).Select(x => x.Key);

            return tops;
        }

        /// <summary>
        /// Returns student ID with the largest difference between their minimum and maximum GPA
        /// </summary>
        /// <param name="students"></param>
        /// <returns>Student ID</returns>
        public int GetStudentIdMostInconsistent(IReadOnlyCollection<Student> students)
        {
            var dict = new Dictionary<int, decimal>();

            foreach (var student in students)
            {
                var min = student.GPARecord.Min();
                var max = student.GPARecord.Max();
                var diff = max - min;
                dict[student.Id] = diff;
            }

            var ordered = dict.OrderByDescending(x => x.Value);
            var id = ordered.First().Key;

            return id;
        }
    }
}
