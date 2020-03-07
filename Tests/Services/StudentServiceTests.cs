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
            var students = GetStudents().ToList();
            var expectedResult = 2005;

            // Act
            var result = _sut.GetHighestAttendanceYear(students);

            // Assert
            result.Should().Be(expectedResult);
        }

        private IEnumerable<Student> GetStudents()
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
