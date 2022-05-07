using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikDotNet
{
    public class MKCommandParameterCollection : List<MKCommandParameter>
    {
        public void Add(string name, string value)
        {
            this.Add(new MKCommandParameter(name, value));
        }
    }
}
