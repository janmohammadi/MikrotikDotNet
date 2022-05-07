using System.Globalization;
using System.Text.RegularExpressions;

namespace MikrotikDotNet.ReponseParser
{
    public class MemberNameHelper
    {
        public static string TrainToPascal(string name)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(name.Replace("-", " ")).Replace(" ",string.Empty).Replace(".",string.Empty);
        }

        public static string PascalToTrain(string name)
        {
            if (name.ToLower() == "mkid")
                return ".id";

            return Regex.Replace(name, "(\\B[A-Z])", "-$1").ToLower();
        }
    }
}