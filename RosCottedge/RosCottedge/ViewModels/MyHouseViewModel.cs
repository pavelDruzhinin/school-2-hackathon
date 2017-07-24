using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.ViewModels
{
    public class MyHouseViewModel
    {
        public List<House> House { get; set; }
        public List<GeneralСlass> GeneralClass { get; set; }
        public User User { get; set; }
    }
}