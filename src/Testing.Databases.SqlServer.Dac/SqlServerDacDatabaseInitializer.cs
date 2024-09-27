//-----------------------------------------------------------------------
// <copyright file="SqlServerDacDatabaseInitializer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Initializer used to initialize the database for the unit tests.
    /// Call the <see cref="Initialize(SqlServerDatabaseInitializer, string, string)"/> method to initialize a database from
    /// a DACPAC file.
    /// </summary>
    /// <remarks>The database will be created the call of the <see cref="Initialize(string, string)"/> method. For the next calls
    /// the database is preserved but all the data are deleted.</remarks>
    public static class SqlServerDacDatabaseInitializer
    {
        /// <summary>
        /// Initialize a SQL Server database from a DACPAC file.
        /// </summary>
        /// <param name="initializer"><see cref="SqlServerDatabaseInitializer"/> which the initialization will be perform on.</param>
        /// <param name="packageName">Full path of the DACPAC file.</param>
        /// <param name="connectionString">Connection string to the SQL Server with administrator rights.</param>
        /// <returns>An instance of the <see cref="SqlServerDatabase"/> which allows to perform query to initialize the data.</returns>
        public static SqlServerDatabase Initialize(this SqlServerDatabaseInitializer initializer, string packageName, string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var server = new SqlServer(connectionString);

            SqlServerDatabase database;

            if (!initializer.IsInitialized)
            {
                database = server.DeployDacPackage(packageName, connectionStringBuilder.InitialCatalog);

                initializer.IsInitialized = true;
            }
            else
            {
                database = server.GetDatabase(connectionStringBuilder.InitialCatalog);
            }

            database.ClearAllData();

            return database;
        }
    }
}
