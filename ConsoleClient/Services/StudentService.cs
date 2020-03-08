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
            ValidateInputCollection(students);
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

        /// <summary>
        /// Returns the year with highest overall GPA
        /// </summary>
        /// <param name="students"></param>
        /// <returns>Year</returns>
        public int GetHighestGPAYear(IReadOnlyCollection<Student> students)
        {
            ValidateInputCollection(students);
            var dictionary = new Dictionary<int, decimal>();

            foreach (var student in students)
            {
                var yearsRange = GetRangeValues(student.StartYear, student.EndYear).ToList();
                var gpaRecords = student.GPARecord.ToList();
                if (yearsRange.Count != gpaRecords.Count)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(gpaRecords)}, {nameof(gpaRecords)}", "Years range is not equal to GPA records.");
                }

                for (var i = 0; i < gpaRecords.Count; i++)
                {
                    var year = yearsRange[i];
                    dictionary[year] = dictionary.GetValueOrDefault(year) + gpaRecords[i];
                }
            }

            var ordered = dictionary.OrderByDescending(x => x.Value);
            var highestGpaYear = ordered.First().Key;

            return highestGpaYear;
        }

        /// <summary>
        /// Returns IDs of the top 10 students with highest overall GPA
        /// </summary>
        /// <returns>List of student's IDs</returns>
        public IEnumerable<int> GetTopTenStudentsWithHighestGPA(IReadOnlyCollection<Student> students)
        {
            var dictionary = new Dictionary<int, decimal>();

            foreach (var student in students)
            {
                dictionary[student.Id] = student.GPARecord.Sum();
            }

            const int topCount = 10;
            var tops = dictionary.OrderByDescending(x => x.Value)
                .Take(topCount).Select(x => x.Key);

            return tops;
        }

        /// <summary>
        /// Returns student ID with the largest difference between their minimum and maximum GPA
        /// </summary>
        /// <param name="students"></param>
        /// <returns>Student ID</returns>
        public int GetStudentIdMostInconsistent(IReadOnlyCollection<Student> students)
        {
            var dictionary = new Dictionary<int, decimal>();

            foreach (var student in students)
            {
                var min = student.GPARecord.Min();
                var max = student.GPARecord.Max();
                var diff = max - min;
                dictionary[student.Id] = diff;
            }

            var ordered = dictionary.OrderByDescending(x => x.Value);
            var id = ordered.First().Key;

            return id;
        }

        private IEnumerable<int> GetRangeValues(int start, int end)
        {
            if (start > end)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Start value is bigger than end value");
            }

            for (var i = start; i <= end; i++)
            {
                yield return i;
            }
        }

        private void ValidateInputCollection(IReadOnlyCollection<Student> students)
        {
            if (students == null)
            {
                throw new ArgumentNullException(nameof(students));
            }

            if (!students.Any())
            {
                throw new ArgumentException("Collection is empty.", nameof(students));
            }
        }
    }
}
