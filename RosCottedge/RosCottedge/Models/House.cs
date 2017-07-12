using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RosCottedge.Models
{
    public class House
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Стоимость")]
        public int Price { get; set; }

        [Display(Name = "Количество человек")]
        public int NumberOfPersons { get; set; }

        [Display(Name = "Область")]
        public string Region { get; set; }

        [Display(Name = "Населённый пункт")]
        public string Locality { get; set; }

        [Display(Name = "Улица")]
        public string Area { get; set; }

        [Display(Name = "Номер дома")]
        public string HouseNumber { get; set; }

        [Display(Name = "Описание")]
        [UIHint("MultilineText")]
        public string Description { get; set; }

        [Display(Name = "Питание")]
        [UIHint("MultilineText")]
        public string Food { get; set; }

        [Display(Name = "Трансфер")]
        [UIHint("MultilineText")]
        public string Transfer { get; set; }

        [Display(Name = "Включённые услуги")]
        [UIHint("MultilineText")]
        public string ServicesIncluded { get; set; }

        [Display(Name = "Услуги за отдельную плату")]
        [UIHint("MultilineText")]
        public string AdditionalServices { get; set; }

        [Display(Name = "Условия проживания")]
        [UIHint("MultilineText")]
        public string Accomodations { get; set; }

        [Display(Name = "Условия бронирования")]
        [UIHint("MultilineText")]
        public string BookingConditions { get; set; }

        public float Rating { get; set; }

        public List<Review> Reviews { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}