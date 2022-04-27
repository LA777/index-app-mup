using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        public IEnumerable<decimal> GPARecord { get; set; }

        public Student()
        {
            GPARecord = Enumerable.Empty<decimal>();
        }
    }
}
