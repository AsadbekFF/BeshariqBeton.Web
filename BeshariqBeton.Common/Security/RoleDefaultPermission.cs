using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Security
{
    public class RoleDefaultPermission
    {
        public UserRole Role { get; set; }

        public List<StandardPermission> Permissions { get; set; } = new List<StandardPermission>();
    }
}
