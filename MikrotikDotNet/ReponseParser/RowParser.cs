using System.Collections.Generic;

namespace MikrotikDotNet.ReponseParser
{
    public class RowParser
    {
        public static Dictionary<string, string> Parse(string row)
        {
            var res = new Dictionary<string, string>();
            //var fields = new List<string>();
            //var values = new List<string>();

            int start = 0, end;
            do
            {
                start = row.IndexOf("=", start + 1);
                end = row.IndexOf("=", start + 1);
                var field = row.Substring(start + 1, end - start - 1);

                start = end;

                //------------------- value
                var value = "";
                var valueLen = row.IndexOf("=", end + 1) - end - 1;
                if (valueLen >= 0)
                    value = row.Substring(end + 1, valueLen);
                else
                    value = row.Substring(end + 1, row.Length - end-1);

                res.Add(field, value);
            } while (end < row.LastIndexOf("="));
            return res;
        }
    }
}