using System;
using System.Collections.Generic;
using System.Linq;
using ERacuni.Shared.Models;
using System.Text;
using System.Threading.Tasks;

namespace ERacuni.Shared.DTO
{
     public class ChartWayOfSelling
    {
        public string Name { get; set; }
        
        public decimal Ratio { get; set; }

        public decimal Total { get; set; }
        public ChartWayOfSelling() { }
        public ChartWayOfSelling(string n, decimal r)
        {
            Name = n;
            Ratio = r;
        }



        public ChartWayOfSelling(string n, decimal r, decimal tr) : this(n, r)
        {
            Total = tr;
        }

    }
}
