using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleValuation.Models
{
    public class ValuationDetails
    {
        public string regNumber {  get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Milage { get; set; }
        public string Manufacturer { get; set; }
    }
}
