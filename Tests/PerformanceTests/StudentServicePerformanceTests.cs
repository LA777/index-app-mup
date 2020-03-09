using AutoFixture;
using ConsoleClient.Models;
using ConsoleClient.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace Tests.PerformanceTests
{
    public class StudentServicePerformanceTests : TestBase
    {
        private readonly IStudentService _sut;
        private const int StudentsCollectionLimit = 100000;
        private static IReadOnlyCollection<Student> _students;

        private static IReadOnlyCollection<Student> Students
        {
            get { return _students ??= GenerateStudents(StudentsCollectionLimit); }
        }


        public StudentServicePerformanceTests()
        {
            _sut = new StudentService();
        }

        [Fact]
        public void GetHighestAttendanceYear_Should_Return_Highest_Attendance_Year_Within_Limited_Time()
        {
            // Arrange
            const long expectedMillisecondsLimit = 100;
            var students = Students;
            var stopWatch = new Stopwatch();

            // Act
            stopWatch.Start();
            _sut.GetHighestAttendanceYear(students);
            stopWatch.Stop();

            // Assert
            stopWatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(expectedMillisecondsLimit);
        }

        [Fact]
        public void GetHighestGpaYear_Should_Return_Highest_GPA_Year_Within_Limited_Time()
        {
            // Arrange
            const long expectedMillisecondsLimit = 150;
            var students = Students;
            var stopWatch = new Stopwatch();

            // Act
            stopWatch.Start();
            var result = _sut.GetHighestGpaYear(students);
            stopWatch.Stop();

            // Assert
            stopWatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(expectedMillisecondsLimit);
        }

        [Fact]
        public void GetTopTenStudentsWithHighestGpa_Should_Return_Id_List_Of_Top_Students_Within_Limited_Time()
        {
            // Arrange
            const long expectedMillisecondsLimit = 100;
            var students = Students;
            var stopWatch = new Stopwatch();

            // Act
            stopWatch.Start();
            var result = _sut.GetTopTenStudentsWithHighestGpa(students);
            stopWatch.Stop();

            // Assert
            stopWatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(expectedMillisecondsLimit);
        }

        [Fact]
        public void GetStudentIdMostInconsistent_Should_Return_Student_Id_Most_Inconsistent_Within_Limited_Time()
        {
            // Arrange
            const long expectedMillisecondsLimit = 100;
            var students = Students;
            var stopWatch = new Stopwatch();

            // Act
            stopWatch.Start();
            var result = _sut.GetStudentIdMostInconsistent(students);
            stopWatch.Stop();

            // Assert
            stopWatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(expectedMillisecondsLimit);
        }

        private static IReadOnlyCollection<Student> GenerateStudents(int count)
        {
            var students = new List<Student>();

            for (var i = 0; i < count; i++)
            {
                var student = Fixture.Create<Student>();
                var random = new Random();
                student.StartYear = random.Next(1990, 2017);
                var yearsDifference = random.Next(0, 4);
                student.EndYear = student.StartYear + yearsDifference;
                student.GPARecord = Fixture.CreateMany<decimal>(yearsDifference + 1);

                students.Add(student);
            }

            return students;
        }

    }
}
