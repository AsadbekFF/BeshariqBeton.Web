using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Enums
{
    public enum PaymentType : byte
    {
        [Display(Name = "Naqd pul")]
        Cash,
        [Display(Name = "Plastik karta")]
        Card
    }
}
