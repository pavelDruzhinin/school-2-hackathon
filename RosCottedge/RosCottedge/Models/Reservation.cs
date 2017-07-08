using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        // ReservationDate - дата оформления бронирования. Удалить, если не нужно.
        public DateTime ReservationDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int HouseId { get; set; }
        public House House { get; set; }
    }
}