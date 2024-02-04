using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Exceptions
{
    public class BetonException : Exception
    {
        public BetonException(Exception exception) : base(exception.Message, exception)
        {

        }

        public BetonException(string message) : base(message)
        {

        }
        public BetonException() { }

        public override string StackTrace => null;
    }
}
