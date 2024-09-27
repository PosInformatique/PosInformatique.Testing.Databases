//-----------------------------------------------------------------------
// <copyright file="EntityFrameworkDatabaseInitializerExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Contains extensions methods on the <see cref="SqlServerDatabaseInitializer"/> to initialize
    /// database using Entity Framework <see cref="DbContext"/>.
    /// </summary>
    public static class EntityFrameworkDatabaseInitializerExtensions
    {
        /// <summary>
        /// Initialize a SQL Server database from a <see cref="DbContext"/> specified in the <paramref name="context"/> argument.
        /// </summary>
        /// <param name="initializer"><see cref="SqlServerDatabaseInitializer"/> which the initialization will be perform on.</param>
        /// <param name="context">Instance of the <see cref="DbContext"/> which represents the database schema to initialize.</param>
        /// <returns>An instance of the <see cref="SqlServerDatabase"/> which allows to perform query to initialize the data.</returns>
        public static SqlServerDatabase Initialize(this SqlServerDatabaseInitializer initializer, DbContext context)
        {
            var connectionString = context.Database.GetConnectionString();

            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var server = new SqlServer(connectionString!);
            var database = server.GetDatabase(connectionStringBuilder.InitialCatalog);

            if (!initializer.IsInitialized)
            {
                server.DeleteDatabase(connectionStringBuilder.InitialCatalog);

                context.Database.EnsureCreated();

                initializer.IsInitialized = true;
            }

            database.ClearAllData();

            return database;
        }
    }
}
