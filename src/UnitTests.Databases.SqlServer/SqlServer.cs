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

            this.Master = new SqlServerDatabase(connectionStringMaster.ToString());
        }

        public SqlServerDatabase Master { get; }

        public SqlServerDatabase CreateEmptyDatabase(string name)
        {
            this.DeleteDatabase(name);
            this.Master.ExecuteNonQuery($"CREATE DATABASE [{name}]");

            return this.GetDatabase(name);
        }

        public void DeleteDatabase(string name)
        {
            this.Master.ExecuteNonQuery($"IF EXISTS (SELECT 1 FROM [sys].[databases] WHERE [name] = '{name}') ALTER DATABASE [{name}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            this.Master.ExecuteNonQuery($"IF EXISTS (SELECT 1 FROM [sys].[databases] WHERE [name] = '{name}') DROP DATABASE [{name}]");
        }

        public SqlServerDatabase GetDatabase(string name)
        {
            var databaseConnectionString = new SqlConnectionStringBuilder(this.originalConnectionString);
            databaseConnectionString.InitialCatalog = name;

            return new SqlServerDatabase(databaseConnectionString.ToString());
        }

        internal SqlServerDatabase GetDatabaseWithAdministratorCredentials(string name)
        {
            var databaseConnectionString = new SqlConnectionStringBuilder(this.Master.ConnectionString);
            databaseConnectionString.InitialCatalog = name;

            return new SqlServerDatabase(databaseConnectionString.ToString());
        }
    }
}
