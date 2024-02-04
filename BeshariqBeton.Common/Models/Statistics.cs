using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Models
{
    public class Statistics
    {
        public List<string> Labels { get; set; } = new List<string>();

        public List<Datasets> Datasets { get; set; } = new List<Datasets>();
    }
}
