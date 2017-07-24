using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RosCottedge.ViewModels
{
    public class EditMyHouseViewModel
    {
        public List<Picture> Pictures { get; set; }
        public List<GeneralСlass> GeneralClass { get; set; }
        public List<Reservation> Reservations { get; set; }

        public House House { get; set; }
        public User User { get; set; }
    }
    public class GeneralСlass
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int HouseId { get; set; }
        public House House { get; set; }
        public bool Tenant { get; set; }
        public bool Landlord { get; set; }                
        public string Comment { get; set; }        
        public float Rating { get; set; }
        public bool Reserv { get; set; }
        public bool Review { get; set; }
    }

}