using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.Models
{
    public class StudentAggregate
    {
        public string YourName { get; set; }

        public string YourEmail { get; set; }

        public int YearWithHighestAttendance { get; set; }

        public int YearWithHighestOverallGpa { get; set; }

        public int StudentIdMostInconsistent { get; set; }

        public IEnumerable<int> Top10StudentIdsWithHighestGpa { get; set; }

        public StudentAggregate()
        {
            Top10StudentIdsWithHighestGpa = Enumerable.Empty<int>();
        }
    }
}
