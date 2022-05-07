using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikDotNet.Exceptions
{
    /// <summary>
    /// Class MKCommandException.
    /// </summary>
    /// <seealso cref="MikrotikDotNet.Exceptions.MKException" />
    public class MKCommandException : MKException
    {
        public MKCommandException()
        {
          
        }

        public MKCommandException(string message)
            : base(message)
        {
        }

        public MKCommandException(string message, CommandFailureTypes errorCategory) : this(message)
        {
            this.ErrorCategory = errorCategory;
        }

        public CommandFailureTypes ErrorCategory { get; set; }
    }
}
