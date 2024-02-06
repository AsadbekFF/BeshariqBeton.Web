using BeshariqBeton.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace BeshariqBeton.Web.ViewModels
{
    public class SaleViewModel
    {
        [Display(Name = "Klient")]
        public int ClientId { get; set; }

        [Display(Name = "Beton turi")]
        public ConcreteProductType ConcreteProductType { get; set; }

        [Display(Name = "Miqdori")]
        public int? Count { get; set; }

        [Display(Name = "Chiqib ketgan vaqti")]
        public DateTime ComeOutDateTime { get; set; }

        [Display(Name = "Qaytib kelgan vaqti")]
        public TimeSpan? ComeInTime { get; set; }

        [Display(Name = "To'lo'v uslubi")]
        public PaymentType PaymentType { get; set; }

        [Display(Name = "To'landi")]
        public bool DebtPaid { get; set; }

        [Display(Name = "Xat raqami")]
        public string LetterNumber { get; set; }

        [Display(Name = "Pastgi qismi miqdori")]
        public int? BottomCount { get; set; }

        [Display(Name = "60 lik miqdori")]
        public int? Sump60Count { get; set; }

        [Display(Name = "90 lik miqdori")]
        public int? Sump90Count { get; set; }

        [Display(Name = "Qopqoq miqodori")]
        public int? CoverCount { get; set; }

        [Display(Name = "Jami narxi")]
        public double TotalPrice { get; set; }
    }
}
