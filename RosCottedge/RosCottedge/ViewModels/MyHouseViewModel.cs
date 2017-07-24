using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.ViewModels
{
    public class MyHouseViewModel
    {
        public IQueryable<House> House { get; set; }
        public IEnumerable<GeneralСlass> GeneralClass { get; set; }
        public User User { get; set; }
    }
}