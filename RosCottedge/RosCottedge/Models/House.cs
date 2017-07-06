using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.Models
{
    public class House
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int NumberOfPersons { get; set; }
        public string Region { get; set; }
        public string Locality { get; set; }
        public string Area { get; set; }
        public string HouseNumber { get; set; }
        public string Description { get; set; }
        public string Food { get; set; }
        public string Transfer { get; set; }
        public string ServicesIncluded { get; set; }
        public string AdditionalServices { get; set; }
        public string Accomodations { get; set; }
        public string BookingConditions { get; set; }
        public decimal Rating { get; set; }
        //Пока что без связи с пользователем, чтобы тот мог регистрировать новые дома.
    }
}