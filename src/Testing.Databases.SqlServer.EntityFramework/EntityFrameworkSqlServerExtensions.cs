//-----------------------------------------------------------------------
// <copyright file="EntityFrameworkSqlServerExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Extensions methods of the <see cref="SqlServer"/> class with additional
    /// helpers for Entity Framework.
    /// </summary>
    public static class EntityFrameworkSqlServerExtensions
    {
        /// <summary>
        /// Deploy a database using a DACPAC file.
        /// </summary>
        /// <remarks>If a database  already exists, it will be deleted.</remarks>
        /// <param name="server"><see cref="SqlServer"/> instance where the database will be created.</param>
        /// <param name="name">Name of the database to create.</param>
        /// <param name="context"><see cref="DbContext"/> which represents the database to create.</param>
        /// <returns>An instance of the <see cref="SqlServerDatabase"/> which represents the deployed database.</returns>
        public static async Task<SqlServerDatabase> CreateDatabaseAsync(this SqlServer server, string name, DbContext context)
        {
            var database = server.GetDatabase(name);

            context.Database.SetConnectionString(database.ConnectionString);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            return database;
        }
    }
}
