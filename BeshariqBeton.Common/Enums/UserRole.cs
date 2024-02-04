using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Enums
{
    public enum UserRole : byte
    {
        [Display(Name = "Foydalanuvchi")]
        User,
        [Display(Name = "Administrator")]
        Admin
    }
}
