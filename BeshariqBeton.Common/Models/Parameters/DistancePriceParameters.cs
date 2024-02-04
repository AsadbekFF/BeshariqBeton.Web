using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Models.Parameters
{
    public class DistancePriceParameters
    {
        public int InitialDistancePrice { get; set; }
        public int InitialDistanceKm { get; set; }

        public int AfterInitialDistancePrice { get; set; }
        public int AfterInitialDistanceKm { get; set; }
    }
}
