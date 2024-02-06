using BeshariqBeton.Common.Entities.Base;
using BeshariqBeton.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Entities
{
    public class Sale : BaseEntity<int>
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public ConcreteProductType ConcreteProductType { get; set; }
        public int Count { get; set; }
        public DateTime ComeOutDateTime { get; set; }
        public DateTime? ComeInDateTime { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool DebtPaid { get; set; }
        public string LetterNumber { get; set; }
        public int? BottomCount { get; set; }
        public int? Sump60Count { get; set; }
        public int? Sump90Count { get; set; }
        public int? CoverCount { get; set; }
        public double TotalPrice { get; set; }
    }
}
