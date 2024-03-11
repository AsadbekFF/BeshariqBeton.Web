using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Enums
{
    public enum SaleFilterType
    {
        [Display(Name = "Klient bo'yicha")]
        ByClient,
        [Display(Name = "Produkta bo'yicha")]
        ByProduct
    }
}
