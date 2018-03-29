using System;
using System.Collections.Generic;
using MikrotikDotNet.Exceptions;
using MikrotikDotNet.ReponseParser;

namespace MikrotikDotNet
{
    /// <summary>
    ///     Represents a Api Command in Mikrotik.
    /// </summary>
    public class MKCommand : IMKCommand
    {
        /// <summary>
        ///     The command text
        /// </summary>
        private string commandText;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MKCommand" /> class.
        /// </summary>
        public MKCommand()
        {
            Parameters = new MKCommandParameterCollection();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MKCommand" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public MKCommand(MKConnection connection)
            : this()
        {
            Connection = connection;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MKCommand" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="commandText">The command text.</param>
        public MKCommand(MKConnection connection, string commandText)
            : this(connection)
        {
            CommandText = commandText;
        }

        /// <summary>
        ///     Gets or sets the command text.
        /// </summary>
        /// <remarks>
        ///     Command text can be in the API format (slash separated) or terminal format (space separated).
        ///     note that all parameters must add in the <see cref="Parameters" /> collection.
        /// </remarks>
        /// <value>The command text.</value>
        /// <example>
        ///     var command="interface pppoe-client add";
        ///     //or
        ///     var command=@"/interface/pppoe-client/add";
        /// </example>
        public string CommandText
        {
            get { return commandText; }
            set
            {
                commandText = value.Replace(" ", @"/");
                if (!commandText.StartsWith("/"))
                    commandText = @"/" + commandText;
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="MKConnection" />.
        /// </summary>
        /// <value>The connection.</value>
        public IMKConnection Connection { get; set; }

        /// <summary>
        ///     Gets or sets the parameters that will pass with the command.
        /// </summary>
        /// <value>The parameters.</value>
        /// <example>
        ///     var cmd = conn.CreateCommand("interface pppoe-client remove");
        ///     cmd.Parameters.Add(".id", "NormalUser");
        ///     cmd.Parameters.Add("disabled", "yes");
        /// </example>
        public MKCommandParameterCollection Parameters { get; set; }


        /// <summary>
        ///     Sends the Command, but does not read the the response.you should execute a read command whenever you want.
        /// </summary>
        /// <remarks>
        ///     this method is suitable for commands that will execute in background for a while, like ping, bandwidth-test etc.
        /// </remarks>
        /// <example>
        /// </example>
        public void ExecuteBackground()
        {
            verifyConnection();
            sendCommand();
        }

        /// <summary>
        ///     Executes the command against the connection.
        /// </summary>
        public void ExecuteNonQuery()
        {
            verifyConnection();
            sendCommand();
            var res = Connection.Read();
            checkResponse(res);
        }

        /// <summary>
        ///     Checks the response.
        /// </summary>
        /// <param name="res">The resource.</param>
        /// <exception cref="MKCommandException"></exception>
        /// <exception cref="MikrotikDotNet.Exceptions.MKCommandException"></exception>
        private void checkResponse(List<string> res)
        {
            var firstRow = res[0];
            if (firstRow.StartsWith("!trap") || firstRow.StartsWith("!fatal"))
            {
                var rowData = RowParser.Parse(firstRow);
                var exMessage = "";
                //if (MKResponseParser.HasField("category", firstRow))
                CommandFailureTypes errorCategory= CommandFailureTypes.GeneralFailure;
                if (rowData.ContainsKey("category"))
                {
                    var cat = int.Parse(rowData["category"]);
                    exMessage = "Category: " + (CommandFailureTypes)cat;
                    errorCategory = (CommandFailureTypes) cat;
                }
                exMessage += "\n" + rowData["message"];
                throw new MKCommandException(exMessage,errorCategory);
            }
            res.Remove("!done");
        }

        /// <summary>
        ///     Sends the command.
        /// </summary>
        private void sendCommand()
        {
            Connection.Send(CommandText);
            foreach (var parameter in Parameters)
            {
                Connection.Send(parameter.Serialize());
            }
            Connection.Push();
        }

        /// <summary>
        ///     Verifies the connection.
        /// </summary>
        /// <exception cref="ArgumentException">Connection object can not be null to excute command</exception>
        /// <exception cref="MikrotikDotNet.Exceptions.MKCommandException">Connection is not open</exception>
        private void verifyConnection()
        {
            if (Connection == null)
                throw new ArgumentException("Connection object can not be null to excute command");

            if (!Connection.IsOpen)
                throw new MKCommandException("Connection is not open");
        }




        /// <summary>
        ///     Gets the command text queries.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>MKCommandParameterCollection.</returns>
        private MKCommandParameterCollection getCommandTextQueries(string commandText)
        {
            var query = new MKCommandParameterCollection();
            var idx = commandText.IndexOf(@"/where/");
            var parameters = commandText.Substring(idx, commandText.Length - idx)
                .Replace(@"/where/", "")
                .Split('=', ',');
            CommandText = commandText.Substring(0, idx);
            for (var i = 0; i < parameters.Length; i += 2)
            {
                query.Add(parameters[i], parameters[i + 1]);
            }

            return query;
        }


        /// <summary>
        ///     Excutes CommandText for read
        /// </summary>
        /// <param name="propList">
        ///     Optional:CSV of properies. limits number of Fileds that will be returned in result. it can
        ///     increase performance.
        /// </param>
        /// <param name="queryConditions">
        ///     Optional: Limits rows by condition on fields. Default logic is AND. You can change it in
        ///     Logic argument of this method
        /// </param>
        /// <param name="logic">The logic.</param>
        /// <returns>A collection of string in raw format of API result.</returns>
        public IEnumerable<string> ExecuteReader(string propList = null,
            MKCommandParameterCollection queryConditions = null, MKQueryLogicOperators logic = MKQueryLogicOperators.And)
        {
            verifyConnection();

            if (!string.IsNullOrEmpty(propList))
            {
                Parameters.Add(".proplist", propList);
            }
            //*******************************************

            if (CommandText.Contains(@"/where/"))
            {
                if (queryConditions == null)
                    queryConditions = new MKCommandParameterCollection();
                queryConditions.AddRange(getCommandTextQueries(CommandText));
            }

            if (queryConditions != null)
            {
                foreach (var q in queryConditions)
                {
                    q.Name = "?" + q.Name;
                    Parameters.Add(q);
                }
                if (queryConditions.Count > 1)
                {
                    switch (logic)
                    {
                        case MKQueryLogicOperators.And:
                            Parameters.Add("?#", "&");
                            break;

                        case MKQueryLogicOperators.Or:
                            Parameters.Add("?#", "|");

                            break;
                    }
                }
            }

            sendCommand();
            var res = Connection.Read();
            checkResponse(res);
            return res;
        }


        /// <summary>
        ///     Executes the command against the connection. reads and Creates Typed objects from the result.
        /// 
        /// </summary>
        /// <typeparam name="TType">The type of the t type.</typeparam>
        /// <param name="queryConditions">The query conditions.</param>
        /// <param name="logic">The logic.</param>
        /// <returns>IEnumerable&lt;TType&gt;.</returns>
        public IEnumerable<TType> ExecuteReader<TType>(MKCommandParameterCollection queryConditions,MKQueryLogicOperators logic)
        {
            var propList = string.Join(",", MKResponseParser.GetMkPropList<TType>());
            var rows = ExecuteReader(propList, queryConditions, logic);

            var result = MKResponseParser.GetList<TType>(rows);
            return result;
        }

        public IEnumerable<TType> ExecuteReader<TType>()
        {
            return ExecuteReader<TType>(null, MKQueryLogicOperators.And);
        }

        /// <summary>
        ///   Executes the command against the connection. reads and Creates dynamic objects from the result.
        /// </summary>
        /// <param name="queryConditions">The query conditions.</param>
        /// <param name="logic">The logic.</param>
        /// <returns>IEnumerable&lt;dynamic&gt;.</returns>
        public IEnumerable<dynamic> ExecuteReaderDynamic(MKCommandParameterCollection queryConditions ,
           MKQueryLogicOperators logic)
        {
            //var propList = string.Join(",", MKResponseParser.GetMkPropList<TType>());
            var rows = ExecuteReader(null, queryConditions, logic);

            var result = MKResponseParser.GetDynamicList(rows);
            return result;
        }

        public virtual IEnumerable<dynamic> ExecuteReaderDynamic()
        {
            return ExecuteReaderDynamic(null, MKQueryLogicOperators.And);
        }
    }
}