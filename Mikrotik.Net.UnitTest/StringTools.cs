namespace Mikrotik.Net.UnitTest
{
    public static class StringTools
    {
        public static string ExtractBetween(this string str, string startTag, string endTag, bool inclusive)
        {
            string rtn = null;

            int s = str.IndexOf(startTag);
            if (s >= 0)
            {
                if (!inclusive)
                    s += startTag.Length;

                int e = str.IndexOf(endTag, s);
                if (e > s)
                {
                    if (inclusive)
                        e += startTag.Length;

                    rtn = str.Substring(s, e - s);
                }
            }

            return rtn;
        }
    }
}