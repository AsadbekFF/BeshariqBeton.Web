using System.ComponentModel.DataAnnotations;

namespace BeshariqBeton.Web.ViewModels
{
    public class ClientViewModel
    {
        [Display(Name = "Nomi")]
        public string Name { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Telefon raqami")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Masofa")]
        public int DistanceKm { get; set; }
    }
}
