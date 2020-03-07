using AutoFixture;
using ConsoleClient.Models;
using ConsoleClient.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Clients
{
    public class StudentServiceTests : TestBase
    {
        private readonly IStudentService _sut;

        public StudentServiceTests()
        {
            _sut = new StudentService();
        }

        [Fact]
        public void GetHighestAttendanceYear_Should_Retrun_Highest_Attendance_Year()
        {
            // Arrange
            var students = GetStudentsHighestAttendanceYear().ToList();
            var expectedResult = 2005;

            // Act
            var result = _sut.GetHighestAttendanceYear(students);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void GetHighestGPAYear_Should_Retrun_Highest_GPA_Year()
        {
            // Arrange
            var students = GetStudentsHighestGPAYear().ToList();
            var expectedResult = 2006;

            // Act
            var result = _sut.GetHighestGPAYear(students);

            // Assert
            result.Should().Be(expectedResult);
        }

        private IEnumerable<Student> GetStudentsHighestGPAYear()
        {
            var yearRanges = new List<(int, int, decimal[])> {
                (2001, 2002, new[] { 2.4m, 3.2m }),
                (2006, 2007, new[] { 2.7m, 3.2m }),
                (2010, 2011, new[] { 2.4m, 3.2m }),
                (2005, 2006, new[] { 2.9m, 3.2m }),
                (2003, 2004, new[] { 2.4m, 3.2m }),
                (2005, 2006, new[] { 2.4m, 3.2m }),
                (2005, 2006, new[] { 2.4m, 3.9m }),
                (2008, 2009, new[] { 2.4m, 3.2m })
            };

            foreach (var yearRange in yearRanges)
            {
                var student = Fixture.Create<Student>();
                student.StartYear = yearRange.Item1;
                student.EndYear = yearRange.Item2;
                student.GPARecord = yearRange.Item3;

                yield return student;
            }
        }


        private IEnumerable<Student> GetStudentsHighestAttendanceYear()
        {
            var yearRanges = new List<(int, int)> {
                (2001, 2002),
                (2006, 2006),
                (2010, 2011),
                (2005, 2006),
                (2003, 2004),
                (2005, 2005),
                (2005, 2006),
                (2008, 2009)
            };

            foreach (var yearRange in yearRanges)
            {
                var student = Fixture.Create<Student>();
                student.StartYear = yearRange.Item1;
                student.EndYear = yearRange.Item2;

                yield return student;
            }
        }
    }
}
