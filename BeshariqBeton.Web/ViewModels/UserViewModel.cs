using BeshariqBeton.Common.Enums;
using BeshariqBeton.Web.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeshariqBeton.Web.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "Foydalanuvchi")]
        [Required(ErrorMessage = DataAnnotationsResources.RequiredAttribute_ValidationError)]
        public string Username { get; set; }

        [Display(Name = "Ismi")]
        [MaxLength(255, ErrorMessage = DataAnnotationsResources.MaxLengthAttribute_ValidationError)]
        public string? FirstName { get; set; }

        [Display(Name = "Familiyasi")]
        [MaxLength(255, ErrorMessage = DataAnnotationsResources.MaxLengthAttribute_ValidationError)]
        public string? Lastname { get; set; }

        [Display(Name = "Parol")]
        [Required(ErrorMessage = DataAnnotationsResources.RequiredAttribute_ValidationError)]
        [MaxLength(255, ErrorMessage = DataAnnotationsResources.MaxLengthAttribute_ValidationError)]
        [PasswordPropertyText(false)]
        public string Password { get; set; }

        [Display(Name = "Telefon raqami")]
        [Required(ErrorMessage = DataAnnotationsResources.RequiredAttribute_ValidationError)]
        //[RegularExpression(@"((+998)?\d(9))?", ErrorMessage = "Noto'g'ri telefon raqami.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Roli")]
        public UserRole Role { get; set; }

        public List<StandardPermissionViewModel> StandardPermissions { get; set; }
    }
}
