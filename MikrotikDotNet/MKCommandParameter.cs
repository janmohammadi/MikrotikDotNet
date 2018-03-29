using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikrotikDotNet.ReponseParser;

namespace MikrotikDotNet
{
    public class MKCommandParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Serialize()
        {
            var prefix = "";
            if (!(Name.StartsWith("?") || Name.StartsWith("=")))
                prefix = "=";

            var opt = "=";
            if (Name.StartsWith("?#"))
                opt = "";

            return $"{prefix}{MemberNameHelper.PascalToTrain(Name)}{opt}{Value}";
        }

        public MKCommandParameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
