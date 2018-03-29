using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;
using MikrotikDotNet.Exceptions;

namespace MikrotikDotNet.ReponseParser
{
    public static class MKResponseParser
    {


        public static TType GetObject<TType>(string row)
        {


            var rowData = RowParser.Parse(row);



            var resObj = Activator.CreateInstance<TType>();

            foreach (var prop in typeof(TType).GetProperties())
            {



                var mkField = MemberNameHelper.PascalToTrain(prop.Name);
                if (!rowData.ContainsKey(mkField))
                    continue;

                var fieldVal = rowData[mkField];

                var propType = prop.PropertyType;

                var propVal = Convert.ChangeType(fieldVal, propType);
                prop.SetValue(resObj, propVal);
            }
            return resObj;
        }


        public static dynamic GetDynamicObject(string row)
        {
            var rowData = RowParser.Parse(row);
            return createDynamicObject(rowData);
        }
        private static dynamic createDynamicObject(Dictionary<string, string> rowData)
        {
            var res = new ExpandoObject() as IDictionary<string, Object>;


            foreach (var field in rowData.Keys)
                res.Add(MemberNameHelper.TrainToPascal(field), rowData[field]);

            return res;
        }






        public static IEnumerable<TType> GetList<TType>(IEnumerable<string> rows)
        {
            var lst = new List<TType>();
            foreach (var row in rows)
            {
                if (!row.StartsWith("!trap"))
                    lst.Add(GetObject<TType>(row));
            }
            return lst;
        }

        public static IEnumerable<string> GetMkPropList<TType>()
        {
            var lst = new List<string>();
            foreach (var prop in typeof(TType).GetProperties())
            {
                lst.Add(MemberNameHelper.PascalToTrain(prop.Name));
            };
            return lst;
        }

        public static IEnumerable<dynamic> GetDynamicList(IEnumerable<string> rows)
        {

            foreach (var row in rows)
            {
                var rowData = RowParser.Parse(row);
                yield return createDynamicObject(rowData);

            }

        }
    }
}
