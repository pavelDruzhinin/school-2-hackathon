using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.ViewModels
{
    public class EditMyHouseViewModel
    {
        public IQueryable<Reservation> Reservations { get; set; }
        public IQueryable<Review> Reviews { get; set; }
        public House House { get; set; }
        public User User { get; set; }
    }
}