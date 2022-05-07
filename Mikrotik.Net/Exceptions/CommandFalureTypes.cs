using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikDotNet.Exceptions
{
    /// <summary>
    /// The values for "Category" field in Api Exceptions.
    /// </summary>
    /// <seealso href="http://wiki.mikrotik.com/wiki/Manual:API#category" target="_blank" />
    public enum CommandFailureTypes
    {
        /// <summary>
        ///  missing item or command
        /// </summary>
        MissingItemOrCommand = 0,
        /// <summary>
        /// argument value failure
        /// </summary>
        ArgumentValueFailure = 1,
        /// <summary>
        /// The execution of command interrupted
        /// </summary>
        ExecutionOfCommandInterrupted = 2,
        /// <summary>
        /// The scripting related failure
        /// </summary>
        ScriptingRelatedFailure = 3,
        /// <summary>
        /// The general failure
        /// </summary>
        GeneralFailure = 4,
        /// <summary>
        /// The API related failure
        /// </summary>
        APIRelatedFailure = 5,
        /// <summary>
        /// The tty related failure
        /// </summary>
        TTYRelatedFailure = 6,
        /// <summary>
        /// The value generated with return command
        /// </summary>
        ValueGeneratedWithReturnCommand = 7
    }
}
