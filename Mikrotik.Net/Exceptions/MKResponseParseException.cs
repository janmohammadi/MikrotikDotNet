using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikDotNet.Exceptions
{
    public class MKResponseParseException : MKException
    {
        public MKResponseParseException()
        {
        }

        public MKResponseParseException(string message)
            : base(message)
        {
        }
    }
}
