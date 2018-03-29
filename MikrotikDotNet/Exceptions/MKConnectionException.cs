using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikDotNet.Exceptions
{
    public class MKConnectionException : MKException
    {
        public MKConnectionException()
        {
        }

        public MKConnectionException(string message)
            : base(message)
        {
        }
    }
}
