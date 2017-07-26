using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RosCottedge.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Имя")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        [Required]
        public string MiddleName { get; set; }
        [Display(Name = "Фамилия")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Электронная почта")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Телефон")]
        [Required]
        public string Phone { get; set; }
        public string Login { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }
        public string Avatar { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<House>Houses { get; set; }
    }
}