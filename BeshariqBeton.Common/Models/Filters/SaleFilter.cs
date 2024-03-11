using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeshariqBeton.Common.Enums;

namespace BeshariqBeton.Common.Models.Filters
{
    public class SaleFilter
    {
        public SaleFilterType SaleFilterType { get; set; }
        public int? ClientId { get; set; }
        public ConcreteProductType? ConcreteProductType { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
