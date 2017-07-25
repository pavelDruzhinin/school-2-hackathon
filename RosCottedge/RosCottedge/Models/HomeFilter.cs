using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.Models
{
    public class HomeFilter
    {
        public string Region { get; set; }
        public int? NumberOfPersons { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public int? StartPrice { get; set; }
        public int? FinishPrice { get; set; }
        public int Page { get; set; }
        public string Sortparam { get; set; }

    }
}