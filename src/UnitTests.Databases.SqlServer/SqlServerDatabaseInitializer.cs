//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseInitializer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using Microsoft.Data.SqlClient;

    public class SqlServerDatabaseInitializer
    {
        public bool IsDeployed { get; set; }

        public SqlServerDatabase Initialize(string packageName, string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var server = new SqlServer(connectionString);

            SqlServerDatabase database;

            if (!this.IsDeployed)
            {
                if (!string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                {
                    server.Master.ExecuteNonQuery($@"
                        IF SUSER_ID ('{connectionStringBuilder.UserID}') IS NULL
                            CREATE LOGIN [{connectionStringBuilder.UserID}] WITH PASSWORD = '{connectionStringBuilder.Password}'");
                }

                database = server.DeployDacPackage(packageName, connectionStringBuilder.InitialCatalog);

                this.IsDeployed = true;
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
