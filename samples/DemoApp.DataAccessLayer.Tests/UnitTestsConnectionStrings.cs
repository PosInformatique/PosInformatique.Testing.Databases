//-----------------------------------------------------------------------
// <copyright file="UnitTestsConnectionStrings.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.DemoApp.DataAccessLayer.Tests
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// This helper allows to centralize the connection string
    /// and the creation of the <see cref="DbContextOptions"/> for the unit tests.
    /// </summary>
    public static class UnitTestsConnectionStrings
    {
        public static DbContextOptions<TContext> CreateDbContextOptions<TContext>(string databaseName)
            where TContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>()
                .UseSqlServer($"Data Source=(localdb)\\DemoApp; Initial Catalog={databaseName}; Integrated Security=True");

            return optionsBuilder.Options;
        }
    }
}
