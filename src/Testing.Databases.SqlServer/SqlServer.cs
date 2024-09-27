//-----------------------------------------------------------------------
// <copyright file="SqlServer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Represents an instance of SQL Server.
    /// </summary>
    public class SqlServer
    {
        private readonly string originalConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServer"/> class using the specified <paramref name="connectionString"/>.
        /// </summary>
        /// <remarks>The connection string specified must be a connection string with the credentials (SQL or Windows) that will be use
        /// to connect to the database.</remarks>
        /// <param name="connectionString">Connection string to the SQL Server instance.</param>
        public SqlServer(string connectionString)
        {
            this.originalConnectionString = connectionString;

            var connectionStringMaster = new SqlConnectionStringBuilder(connectionString);
            connectionStringMaster.InitialCatalog = "master";

            this.Master = new SqlServerDatabase(this, connectionStringMaster.ToString());
        }

        /// <summary>
        /// Gets an instance of <see cref="SqlServerDatabase"/> which allows to perform
        /// queries on the <c>master</c> database with the administrator credentials.
        /// </summary>
        public SqlServerDatabase Master { get; }

        /// <summary>
        /// Creates an empty database in the SQL Server instance with the specified <paramref name="name"/>.
        /// If the database already exists, it will be delete.
        /// </summary>
        /// <param name="name">Name of the database to create.</param>
        /// <returns>An instance of <see cref="SqlServerDatabase"/> which allows to execute SQL commands/queries.</returns>
        public SqlServerDatabase CreateEmptyDatabase(string name)
        {
            this.DeleteDatabase(name);
            this.Master.ExecuteNonQuery($"CREATE DATABASE [{name}]");

            return this.GetDatabase(name);
        }

        /// <summary>
        /// Deletes a database in the SQL server instance with the specified <paramref name="name"/>.
        /// If the database does not exists, no exception is thrown.
        /// </summary>
        /// <param name="name">Name of the database to delete.</param>
        public void DeleteDatabase(string name)
        {
            this.Master.ExecuteNonQuery($"IF EXISTS (SELECT 1 FROM [sys].[databases] WHERE [name] = '{name}') ALTER DATABASE [{name}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            this.Master.ExecuteNonQuery($"IF EXISTS (SELECT 1 FROM [sys].[databases] WHERE [name] = '{name}') DROP DATABASE [{name}]");
        }

        /// <summary>
        /// Gets an instance of the <see cref="SqlServerDatabase"/> for the database specified with the <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the database which the <see cref="SqlServerDatabase"/> instance will be retrieve for.</param>
        /// <returns>An instance of <see cref="SqlServerDatabase"/> for the database specified with the <paramref name="name"/> which allows to execute commands / queries.</returns>
        public SqlServerDatabase GetDatabase(string name)
        {
            var databaseConnectionString = new SqlConnectionStringBuilder(this.originalConnectionString);
            databaseConnectionString.InitialCatalog = name;

            return new SqlServerDatabase(this, databaseConnectionString.ToString());
        }
    }
}
