using BeshariqBeton.Common.Entities.Base;
using BeshariqBeton.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Entities
{
    public class UserVersion : BaseEntity<int>
    {
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public ActionType ActionType { get; set; }
        public int Version { get; set; }
        public string Content { get; set; }
    }
}
