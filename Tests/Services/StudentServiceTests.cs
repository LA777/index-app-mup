using AutoFixture;
using ConsoleClient.Models;
using ConsoleClient.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.Services
{
    public class StudentServiceTests : TestBase
    {
        private readonly IStudentService _sut;

        public StudentServiceTests()
        {
            _sut = new StudentService();
        }

        [Fact]
        public void GetHighestAttendanceYear_Should_Return_Highest_Attendance_Year()
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
        public void GetHighestGpaYear_Should_Return_Highest_GPA_Year()
        {
            // Arrange
            var students = GetStudentsHighestGpaYear().ToList();
            var expectedResult = 2006;

            // Act
            var result = _sut.GetHighestGpaYear(students);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void GetTopTenStudentsWithHighestGpa_Should_Return_Id_List_Of_Top_Students()
        {
            // Arrange
            var students = GetStudentsHighestGpa().ToList();
            var expectedResult = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 28, 10 };

            // Act
            var result = _sut.GetTopTenStudentsWithHighestGpa(students);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetStudentIdMostInconsistent_Should_Return_Student_Id_Most_Inconsistent()
        {
            // Arrange
            var students = GetStudentsMostInconsistent().ToList();
            var expectedResult = 7;

            // Act
            var result = _sut.GetStudentIdMostInconsistent(students);

            // Assert
            result.Should().Be(expectedResult);
        }

        private IEnumerable<Student> GetStudentsMostInconsistent()
        {
            var yearRanges = new List<(int, int, int, decimal[])> {
                (1, 2001, 2002, new[] { 2.4m, 3.2m }),
                (2, 2006, 2007, new[] { 2.7m, 3.2m }),
                (3, 2010, 2011, new[] { 2.4m, 3.2m }),
                (4, 2005, 2006, new[] { 2.9m, 3.2m }),
                (5, 2003, 2004, new[] { 2.4m, 3.2m }),
                (6, 2005, 2006, new[] { 2.4m, 3.2m }),
                (7, 2005, 2006, new[] { 1.4m, 4.4m }),
                (8, 2008, 2009, new[] { 2.4m, 3.2m }),
                (9, 2008, 2009, new[] { 2.4m, 3.2m }),
                (10, 2008, 2009, new[] { 2.4m, 4.2m }),
                (18, 2008, 2009, new[] { 2.4m, 3.2m }),
                (28, 2008, 2009, new[] { 4.4m, 3.2m })
            };

            foreach (var yearRange in yearRanges)
            {
                var student = Fixture.Create<Student>();
                student.Id = yearRange.Item1;
                student.StartYear = yearRange.Item2;
                student.EndYear = yearRange.Item3;
                student.GPARecord = yearRange.Item4;

                yield return student;
            }
        }

        private IEnumerable<Student> GetStudentsHighestGpa()
        {
            var yearRanges = new List<(int, int, int, decimal[])> {
                (1, 2001, 2002, new[] { 2.4m, 3.2m }),
                (2, 2006, 2007, new[] { 2.7m, 3.2m }),
                (3, 2010, 2011, new[] { 2.4m, 3.2m }),
                (4, 2005, 2006, new[] { 2.9m, 3.2m }),
                (5, 2003, 2004, new[] { 2.4m, 3.2m }),
                (6, 2005, 2006, new[] { 2.4m, 3.2m }),
                (7, 2005, 2006, new[] { 2.4m, 3.9m }),
                (8, 2008, 2009, new[] { 2.4m, 3.2m }),
                (9, 2008, 2009, new[] { 2.4m, 3.2m }),
                (10, 2008, 2009, new[] { 2.4m, 4.2m }),
                (18, 2008, 2009, new[] { 2.4m, 3.2m }),
                (28, 2008, 2009, new[] { 4.4m, 3.2m })
            };

            foreach (var yearRange in yearRanges)
            {
                var student = Fixture.Create<Student>();
                student.Id = yearRange.Item1;
                student.StartYear = yearRange.Item2;
                student.EndYear = yearRange.Item3;
                student.GPARecord = yearRange.Item4;

                yield return student;
            }
        }


        private IEnumerable<Student> GetStudentsHighestGpaYear()
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
