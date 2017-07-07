using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        //Patronymic заменил на MiddleName
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<House>Houses { get; set; }
    }
}