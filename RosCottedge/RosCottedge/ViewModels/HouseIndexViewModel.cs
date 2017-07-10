using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RosCottedge.Models;

namespace RosCottedge.ViewModels
{
    public class HouseIndexViewModel
    {
        public List<Review> Reviews { get; set; }
        public House House { get; set; }
    }
}