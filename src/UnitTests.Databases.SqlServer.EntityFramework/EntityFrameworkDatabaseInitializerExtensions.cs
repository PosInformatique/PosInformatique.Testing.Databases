//-----------------------------------------------------------------------
// <copyright file="EntityFrameworkDatabaseInitializerExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    public static class EntityFrameworkDatabaseInitializerExtensions
    {
        public static SqlServerDatabase Initialize<TContext>(this SqlServerDatabaseInitializer initializer, TContext context)
            where TContext : DbContext
        {
            var connectionString = context.Database.GetConnectionString();

            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var server = new SqlServer(connectionString!);
            var database = server.GetDatabase(connectionStringBuilder.InitialCatalog);

            if (!initializer.IsDeployed)
            {
                server.DeleteDatabase(connectionStringBuilder.InitialCatalog);

                context.Database.EnsureCreated();

                if (!string.IsNullOrWhiteSpace(connectionStringBuilder.UserID))
                {
                    server.Master.ExecuteNonQuery($@"
                        IF SUSER_ID ('{connectionStringBuilder.UserID}') IS NULL
                            CREATE LOGIN [{connectionStringBuilder.UserID}] WITH PASSWORD = '{connectionStringBuilder.Password}'");
                }

                initializer.IsDeployed = true;
            }

            database.ClearAllData();

            return database;
        }
    }
}
