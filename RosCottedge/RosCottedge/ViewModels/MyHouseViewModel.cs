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
        public IQueryable<Review> Review { get; set; }
        public IQueryable<Reservation> Reservation { get; set; }
    }
}