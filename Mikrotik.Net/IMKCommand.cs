using System.Collections.Generic;

namespace MikrotikDotNet
{
    public interface IMKCommand
    {
        string CommandText { get; set; }
        IMKConnection Connection { get; set; }
        MKCommandParameterCollection Parameters { get; set; }

        void ExecuteBackground();
        void ExecuteNonQuery();
        IEnumerable<string> ExecuteReader(string propList = null, MKCommandParameterCollection queryConditions = null, MKQueryLogicOperators logic = MKQueryLogicOperators.And);
        IEnumerable<TType> ExecuteReader<TType>(MKCommandParameterCollection queryConditions, MKQueryLogicOperators logic );
        IEnumerable<TType> ExecuteReader<TType>();
        IEnumerable<dynamic> ExecuteReaderDynamic(MKCommandParameterCollection queryConditions , MKQueryLogicOperators logic);

        IEnumerable<dynamic> ExecuteReaderDynamic();
    }
}