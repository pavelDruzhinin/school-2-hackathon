using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RosCottedge.Models.Login_register
{
    public class Login
    {
        [Required]
        [Display(Name = "Логин")]
        public string LoginName { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}