using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HouseId { get; set; }
        //Даты пока прописал строковыми значениями, так как не знаю, как их лучше хранить. Исправить, если неверно.
        // ReservationDate - дата оформления бронирования. Удалить, если не нужно.
        public string ReservationDate { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureDate { get; set; }
        public User User { get; set; }
        public House House { get; set; }
    }
}