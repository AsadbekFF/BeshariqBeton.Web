using System.ComponentModel.DataAnnotations;

namespace BeshariqBeton.Web.ViewModels
{
    public class PriceSettingsViewModel
    {
        [Display(Name = "Logo")]
        public string? LogoPath { get; set; }


        [Display(Name = "Sement narxi (kg)")]
        public int СementPriceKg { get; set; }
        [Display(Name = "Qumni narxi (metr kub)")]
        public int SandPriceM3 { get; set; }
        [Display(Name = "Sheben narxi (metr kub)")]
        public int ShabenPriceM3 { get; set; }
        [Display(Name = "Ximikat narxi (kg)")]
        public int СhemicalPriceKg { get; set; }


        [Display(Name = "Boshlang'ich masofa narxi")]
        public int InitialDistancePrice { get; set; }
        [Display(Name = "Masofa (km)")]
        public int InitialDistanceKm { get; set; }
        [Display(Name = "Boshlang'ichdan keyingi narx")]
        public int AfterInitialDistancePrice { get; set; }
        [Display(Name = "Masofa (km)")]
        public int AfterInitialDistanceKm { get; set; }


        [Display(Name = "Plita narxi")]
        public int PlatePrice { get; set; }
        [Display(Name = "Shlakoblok narxi")]
        public int CinderBlockPrice { get; set; }
        [Display(Name = "Stavka %")]
        public int InterestRate { get; set; }
        [Display(Name = "Pastgi qismi narxi")]
        public int BottomPrice { get; set; }
        [Display(Name = "60 lik narxi")]
        public int Sump60Price { get; set; }
        [Display(Name = "90 lik narxi")]
        public int Sump90Price { get; set; }
        [Display(Name = "Qopqoq narxi")]
        public int CoverPrice { get; set; }


        [Display(Name = "Sement miqdori (kg)")]
        public float СementWeightKg100 { get; set; }
        [Display(Name = "Qum miqdori (metr kub)")]
        public float SandVolume100 { get; set; }
        [Display(Name = "Sheben miqdori (metr kub)")]
        public float ShabenVolume100 { get; set; }
        [Display(Name = "Ximikat miqdori (kg)")]
        public float СhemicalKg100 { get; set; }

        [Display(Name = "Sement miqdori (kg)")]
        public float СementWeightKg150 { get; set; }
        [Display(Name = "Qum miqdori (metr kub)")]
        public float SandVolume150 { get; set; }
        [Display(Name = "Sheben miqdori (metr kub)")]
        public float ShabenVolume150 { get; set; }
        [Display(Name = "Ximikat miqdori (kg)")]
        public float СhemicalKg150 { get; set; }

        [Display(Name = "Sement miqdori (kg)")]
        public float СementWeightKg200 { get; set; }
        [Display(Name = "Qum miqdori (metr kub)")]
        public float SandVolume200 { get; set; }
        [Display(Name = "Sheben miqdori (metr kub)")]
        public float ShabenVolume200 { get; set; }
        [Display(Name = "Ximikat miqdori (kg)")]
        public float СhemicalKg200 { get; set; }

        [Display(Name = "Sement miqdori (kg)")]
        public float СementWeightKg250 { get; set; }
        [Display(Name = "Qum miqdori (metr kub)")]
        public float SandVolume250 { get; set; }
        [Display(Name = "Sheben miqdori (metr kub)")]
        public float ShabenVolume250 { get; set; }
        [Display(Name = "Ximikat miqdori (kg)")]
        public float СhemicalKg250 { get; set; }

        [Display(Name = "Sement miqdori (kg)")]
        public float СementWeightKg300 { get; set; }
        [Display(Name = "Qum miqdori (metr kub)")]
        public float SandVolume300 { get; set; }
        [Display(Name = "Sheben miqdori (metr kub)")]
        public float ShabenVolume300 { get; set; }
        [Display(Name = "Ximikat miqdori (kg)")]
        public float СhemicalKg300 { get; set; }

        [Display(Name = "Sement miqdori (kg)")]
        public float СementWeightKg350 { get; set; }
        [Display(Name = "Qum miqdori (metr kub)")]
        public float SandVolume350 { get; set; }
        [Display(Name = "Sheben miqdori (metr kub)")]
        public float ShabenVolume350 { get; set; }
        [Display(Name = "Ximikat miqdori (kg)")]
        public float СhemicalKg350 { get; set; }

        [Display(Name = "Sement miqdori (kg)")]
        public float СementWeightKg400 { get; set; }
        [Display(Name = "Qum miqdori (metr kub)")]
        public float SandVolume400 { get; set; }
        [Display(Name = "Sheben miqdori (metr kub)")]
        public float ShabenVolume400 { get; set; }
        [Display(Name = "Ximikat miqdori (kg)")]
        public float СhemicalKg400 { get; set; }
    }
}
