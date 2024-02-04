using BeshariqBeton.Common.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Entities
{
    public class StandardPermission : BaseEntity<int>
    {
        /// <summary>
        /// Permission name
        /// </summary>
        [Required, MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// A system name of a permission to be used in code
        /// </summary>
        [Required, MaxLength(255)]
        public string SystemName { get; set; }
    }
}
