//-----------------------------------------------------------------------
// <copyright file="SqlCmdDatabaseInitializer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Initializer used to initialize the database for the tests.
    /// Call the <see cref="Initialize(SqlServerDatabaseInitializer, string, string, SqlCmdRunScriptSettings?)"/> method to initialize a database from
    /// a T-SQL script file using <c>sqlcmd</c>.
    /// </summary>
    /// <remarks>The database will be created the call of the <see cref="Initialize(SqlServerDatabaseInitializer, string, string, SqlCmdRunScriptSettings?)"/> method. For the next calls
    /// the database is preserved but all the data are deleted.</remarks>
    public static class SqlCmdDatabaseInitializer
    {
        /// <summary>
        /// Initialize a SQL Server database by executing the T-SQL script specified in the <paramref name="fileName"/> argument.
        /// The script will be executed on the <c>master</c> database specified in the <paramref name="connectionString"/>, so
        /// you have to switch the connection if need in your script using the T-SQL <c>USE</c> directive.
        /// </summary>
        /// <param name="initializer"><see cref="SqlServerDatabaseInitializer"/> which the initialization will be perform on.</param>
        /// <param name="fileName">Full path of the T-SQL file to execute.</param>
        /// <param name="connectionString">Connection string to the SQL Server with administrator rights.</param>
        /// <param name="settings">Additionnal settings to run the <c>sqlcmd</c> tool.</param>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="initializer"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="fileName"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If the specified <paramref name="connectionString"/> argument is <see langword="null"/>.</exception>
        /// <exception cref="FileNotFoundException">If no file exists with the specified <paramref name="fileName"/> argument.</exception>
        /// <returns>An instance of the <see cref="SqlServerDatabase"/> which allows to perform query to initialize the data.</returns>
        public static SqlServerDatabase Initialize(this SqlServerDatabaseInitializer initializer, string fileName, string connectionString, SqlCmdRunScriptSettings? settings = null)
        {
            ArgumentNullException.ThrowIfNull(initializer, nameof(initializer));
            ArgumentNullException.ThrowIfNull(fileName, nameof(fileName));
            ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"Could not find file '{fileName}'", fileName);
            }

            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var server = new SqlServer(connectionString);

            SqlServerDatabase database;

            if (!initializer.IsInitialized)
            {
                server.Master.RunScript(fileName, settings);

                initializer.IsInitialized = true;
            }

            database = server.GetDatabase(connectionStringBuilder.InitialCatalog);
            database.ClearAllData();

            return database;
        }
    }
}
