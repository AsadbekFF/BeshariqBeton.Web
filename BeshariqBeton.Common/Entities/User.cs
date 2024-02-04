using BeshariqBeton.Common.Entities.Base;
using BeshariqBeton.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Entities
{
    public class User : BaseEntity<int>
    {
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [MaxLength(255)]
        public string? FirstName { get; set; }

        [MaxLength(255)]
        public string? Lastname { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        public UserRole Role { get; set; }

        public List<UserStandardPermission> StandardPermissions { get; set; } = new List<UserStandardPermission>();
        public DateTime Created { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
