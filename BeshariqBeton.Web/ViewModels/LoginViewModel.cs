using BeshariqBeton.Web.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeshariqBeton.Web.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Foydalanuvchi")]
        [Required(ErrorMessage = DataAnnotationsResources.RequiredAttribute_ValidationError)]
        public string Username { get; set; }

        [Display(Name = "Parol")]
        [Required(ErrorMessage = DataAnnotationsResources.RequiredAttribute_ValidationError)]
        [PasswordPropertyText(false)]
        public string Password { get; set; }

        [Display(Name = "Esda qolsin")]
        public bool RememberMe { get; set; }
    }
}
