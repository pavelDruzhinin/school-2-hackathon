using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.ViewModels
{
    public class MyTripsViewModel
    {
        public IQueryable<Reservation> ReservationHistory { get; set; }
        public IQueryable<Reservation> ReservationDelete { get; set; }

    }
}