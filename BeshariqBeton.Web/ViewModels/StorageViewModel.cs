using BeshariqBeton.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace BeshariqBeton.Web.ViewModels
{
    public class StorageViewModel
    {
        [Display(Name = "Sement miqdori")]
        public double CementRemainKg { get; set; }
        [Display(Name = "Qum miqdori")]
        public double SandRemainM3 { get; set; }
        [Display(Name = "Sheben miqdori")]
        public double ShebenRemainM3 { get; set; }
        [Display(Name = "Ximikat miqdori")]
        public double ChemicalRemainKg { get; set; }
    }
}
