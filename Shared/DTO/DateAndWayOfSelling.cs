using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERacuni.Shared.DTO
{
    public class DateAndWayOfSelling
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public string wayOfSelling { get; set; }

        public string TypeOfDate { get; set; }

        public DateAndWayOfSelling() { }
        public DateAndWayOfSelling(DateTime s,DateTime e, string w, string t)
        {
            start = s;
            end = e;
            wayOfSelling = w;
            TypeOfDate = t;
        }

    }
}
