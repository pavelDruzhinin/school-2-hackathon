using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Adress { get; set; }
        public int HouseId { get; set; }
        public House House { get; set; }
    }
}