//-----------------------------------------------------------------------
// <copyright file="SqlServer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using Microsoft.Data.SqlClient;

    public class SqlServer
    {
        private readonly string originalConnectionString;

        public SqlServer(string connectionString)
        {
            this.originalConnectionString = connectionString;

            var connectionStringMaster = new SqlConnectionStringBuilder(connectionString);
            connectionStringMaster.InitialCatalog = "master";
            connectionStringMaster.IntegratedSecurity = true;
            connectionStringMaster.Remove("User ID");
            connectionStringMaster.Remove("Password");

            this.ConnectionString = connectionStringMaster.ToString();
        }

        public string ConnectionString { get; }

        public SqlServerDatabase CreateEmptyDatabase(string name)
        {
            this.DeleteDatabase(name);
            this.ExecuteNonQuery($"CREATE DATABASE [{name}]");

            return this.GetDatabase(name);
        }

        public void DeleteDatabase(string name)
        {
            this.ExecuteNonQuery($"IF EXISTS (SELECT 1 FROM [sys].[databases] WHERE [name] = '{name}') ALTER DATABASE [{name}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            this.ExecuteNonQuery($"IF EXISTS (SELECT 1 FROM [sys].[databases] WHERE [name] = '{name}') DROP DATABASE [{name}]");
        }

        public SqlServerDatabase GetDatabase(string name)
        {
            var databaseConnectionString = new SqlConnectionStringBuilder(this.originalConnectionString);
            databaseConnectionString.InitialCatalog = name;

            return new SqlServerDatabase(databaseConnectionString.ToString());
        }

        internal static int ExecuteNonQuery(string command, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var dbCommand = connection.CreateCommand())
                {
                    dbCommand.CommandText = command;

                    return dbCommand.ExecuteNonQuery();
                }
            }
        }

        internal SqlServerDatabase GetDatabaseWithMasterCredentials(string name)
        {
            var databaseConnectionString = new SqlConnectionStringBuilder(this.ConnectionString);
            databaseConnectionString.InitialCatalog = name;

            return new SqlServerDatabase(databaseConnectionString.ToString());
        }

        internal int ExecuteNonQuery(string command)
        {
            return ExecuteNonQuery(command, this.ConnectionString);
        }
    }
}
