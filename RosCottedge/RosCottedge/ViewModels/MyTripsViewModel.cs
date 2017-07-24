using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.ViewModels
{
    public class MyTripsViewModel
    {
        public List<Reservation> ReservationHistory { get; set; }
        public List<Reservation> ReservationDelete { get; set; }
        public User User { get; set; }

    }
}