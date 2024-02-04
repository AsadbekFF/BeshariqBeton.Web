using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Utilities
{
    public static class EnumExtensions
    {
        public static object ToValue(this object value)
        {
            return Convert.ChangeType(value, System.Enum.GetUnderlyingType(value.GetType()));
        }
    }
}
