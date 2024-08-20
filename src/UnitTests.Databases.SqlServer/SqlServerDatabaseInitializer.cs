//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseInitializer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    public class SqlServerDatabaseInitializer
    {
        private bool isDeployed;

        public SqlServerDatabase Initialize(string packageName, string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var server = new SqlServer(connectionString);

            SqlServerDatabase database;

            if (!this.isDeployed)
            {
                if (!string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                {
                    server.Master.ExecuteNonQuery($@"
                        IF SUSER_ID ('{connectionStringBuilder.UserID}') IS NULL
                            CREATE LOGIN [{connectionStringBuilder.UserID}] WITH PASSWORD = '{connectionStringBuilder.Password}'");
                }

                database = server.DeployDacPackage(packageName, connectionStringBuilder.InitialCatalog);

                this.isDeployed = true;
            }
            else
            {
                database = server.GetDatabase(connectionStringBuilder.InitialCatalog);
            }

            ClearAllData(database.AsAdministrator());

            return database;
        }

        public SqlServerDatabase Initialize<TContext>(TContext context)
            where TContext : DbContext
        {
            var connectionString = context.Database.GetConnectionString();

            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var server = new SqlServer(connectionString!);
            var database = server.GetDatabase(connectionStringBuilder.InitialCatalog);

            if (!this.isDeployed)
            {
                server.DeleteDatabase(connectionStringBuilder.InitialCatalog);

                // Change the connection with administrator rights.
                context.Database.SetConnectionString(database.AsAdministrator().ConnectionString);
                context.Database.EnsureCreated();
                context.Database.SetConnectionString(connectionString);

                if (!string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                {
                    server.Master.ExecuteNonQuery($@"
                        IF SUSER_ID ('{connectionStringBuilder.UserID}') IS NULL
                            CREATE LOGIN [{connectionStringBuilder.UserID}] WITH PASSWORD = '{connectionStringBuilder.Password}'");
                }

                this.isDeployed = true;
            }

            ClearAllData(database.AsAdministrator());

            return database;
        }

        private static void ClearAllData(SqlServerDatabase database)
        {
            database.ExecuteNonQuery("EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all'");

            database.ExecuteNonQuery("EXEC sp_msforeachtable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'");

            // Re-initialize the seed of the IDENTITY columns.
            // For each table which contains an IDENTITY column, execute the following SQL statement:
            //   DBCC CHECKIDENT ('[<schema>].[<table>]', RESEED, <seed>)
            database.ExecuteNonQuery(@"
                DECLARE @sqlcmd VARCHAR(MAX);

                SET @sqlcmd = (
	                SELECT STRING_AGG(CAST('DBCC CHECKIDENT (''[' + [s].[name] + '].[' + [t].[name] + ']'', RESEED, ' + CAST([ic].[seed_value] AS VARCHAR(20)) + ')' AS NVARCHAR(MAX)),';' + CHAR(10)) WITHIN GROUP (ORDER BY [t].[name])
	                FROM
		                [sys].[schemas] AS [s],
		                [sys].[tables] AS [t],
		                [sys].[columns] AS [c],
		                [sys].[identity_columns] AS [ic]
	                WHERE
			                [s].[schema_id] = [t].[schema_id]
		                AND [t].[object_id] = [c].[object_id]
		                AND [c].[is_identity] = 1
		                AND [c].[object_id] = [ic].[object_id]
		                AND [c].[column_id] = [ic].[column_id]
                )

                EXEC (@sqlcmd)");

            database.ExecuteNonQuery("EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all'");
        }
    }
}
