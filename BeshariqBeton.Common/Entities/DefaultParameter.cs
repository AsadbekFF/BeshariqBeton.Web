using BeshariqBeton.Common.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Entities
{
    public class DefaultParameter : BaseEntity<int>
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
