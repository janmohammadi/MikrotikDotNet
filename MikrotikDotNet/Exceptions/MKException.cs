using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikDotNet.Exceptions
{
    public class MKException : Exception
    {
        public MKException()
        {
        }

        public MKException(string msg)
            : base(msg)
        {
        }
    }
}
