using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Models
{
    public class Datasets
    {
        public string Label { get; set; }
        public List<int> Data { get; set; } = new List<int>();
        public string BorderColor { get; set; }
    }
}
