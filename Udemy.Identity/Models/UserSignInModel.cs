using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Udemy.Identity.Models
{
    public class UserSignInModel
    {

        [Required(ErrorMessage ="Kullanıcı adi gereklidir")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Şifre gereklidir")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }

    }
}
