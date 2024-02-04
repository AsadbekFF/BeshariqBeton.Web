using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Entities
{
    public class UserStandardPermission
    {
        public int StandardPermissionId { get; set; }
        public StandardPermission StandardPermission { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
