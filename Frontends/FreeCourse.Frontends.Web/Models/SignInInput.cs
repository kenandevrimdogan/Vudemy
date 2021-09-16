using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Frontends.Web.Models
{
    public class SignInInput
    {
        [Display(Name = "Email Adresiniz")]
        public string Email { get; set; }

        [Display(Name = "Şifreniz")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool IsRemember { get; set; }
    }
}
