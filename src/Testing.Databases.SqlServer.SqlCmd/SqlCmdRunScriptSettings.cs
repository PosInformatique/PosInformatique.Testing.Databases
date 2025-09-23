//-----------------------------------------------------------------------
// <copyright file="SqlCmdRunScriptSettings.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    /// <summary>
    /// Contains additional settings when runing the <c>sqlcmd</c>.
    /// </summary>
    public class SqlCmdRunScriptSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCmdRunScriptSettings"/> class.
        /// </summary>
        public SqlCmdRunScriptSettings()
        {
            this.Variables = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets a collection of variables and associated values which will be applied
        /// on the script to run. The variables can be referenced in the T-SQL script
        /// using the <c>$(Variable)</c> syntax.
        /// </summary>
        public IDictionary<string, string> Variables { get; }
    }
}
