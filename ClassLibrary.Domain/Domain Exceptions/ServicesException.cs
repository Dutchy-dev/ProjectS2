using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Domain_Exceptions
{
    public class ServicesException : Exception
    {
        public ServicesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
