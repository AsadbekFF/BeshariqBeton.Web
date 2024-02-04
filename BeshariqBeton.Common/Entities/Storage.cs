using BeshariqBeton.Common.Entities.Base;
using BeshariqBeton.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Entities
{
    public class Storage : BaseEntity<int>
    {
        public int CementWeightKg { get; set; }
        public int SandWeightKg { get; set; }
        public int ShebenWeightKg { get; set; }
        public int ChemicalWeightKg { get; set; }
        public string MachineNumber { get; set; }
        public StorageType StorageType { get; set; }
        public DateTime DateTime { get; set; }
    }
}
