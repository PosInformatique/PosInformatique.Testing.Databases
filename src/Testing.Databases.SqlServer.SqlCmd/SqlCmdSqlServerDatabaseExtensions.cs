//-----------------------------------------------------------------------
// <copyright file="SqlCmdSqlServerDatabaseExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Contains extensions methods for the <see cref="SqlServerDatabase"/> to run script using the SQL Server <c>sqlcmd</c> tool.
    /// </summary>
    public static class SqlCmdSqlServerDatabaseExtensions
    {
        /// <summary>
        /// Run the T-SQL script specified in <paramref name="fileName"/> with the <c>sqlcmd</c> tool.
        /// </summary>
        /// <param name="database"><paramref name="database"/> where the script will be executed on.</param>
        /// <param name="fileName">T-SQL script to execute on the <paramref name="database"/>.</param>
        /// <param name="settings">Additional settings to run the script.</param>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="database"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="fileName"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="FileNotFoundException">If no file exists with the specified <paramref name="fileName"/> argument.</exception>
        /// <exception cref="SqlCmdException">If an error has been occured when running the T-SQL script. Check the <see cref="SqlCmdException.Output"/>
        /// to retrieve the output result of the script execution.</exception>
        public static void RunScript(this SqlServerDatabase database, string fileName, SqlCmdRunScriptSettings? settings = null)
        {
            Guard.ThrowIfNull(database, nameof(database));
            Guard.ThrowIfNull(fileName, nameof(fileName));

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"Could not find file '{fileName}'", fileName);
            }

            if (settings is null)
            {
                settings = new SqlCmdRunScriptSettings();
            }

            var connectionStringBuilder = new SqlConnectionStringBuilder(database.ConnectionString);

            using (var sqlCmdProcess = SqlCmdProcess.RunScript(connectionStringBuilder, fileName, settings))
            {
                var exitCode = sqlCmdProcess.WaitForExit();

                if (exitCode != 0)
                {
                    throw new SqlCmdException($"Some errors has been occurred when executing the '{fileName}'.{Environment.NewLine}{Environment.NewLine}-- Output --{Environment.NewLine}{sqlCmdProcess.Output}", sqlCmdProcess.Output);
                }
            }
        }
    }
}
