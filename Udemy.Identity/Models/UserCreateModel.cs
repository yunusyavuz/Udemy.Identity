using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Udemy.Identity.Models
{
    public class UserCreateModel
    {
       [Required(ErrorMessage ="Kullanıcı ismi gereklidir.")]
       public string Username { get; set; }

        [Required(ErrorMessage ="Password alanı gereklidir")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage ="Lütfen bir email formatı giriniz")]
        [Required(ErrorMessage ="Email gereklidir")]
        public string Email { get; set; }


        [Compare("Password", ErrorMessage ="Parolalar eşleşmiyor")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage ="Cinsiyet gereklidir")]
        public string Gender { get; set; }

    }
}
