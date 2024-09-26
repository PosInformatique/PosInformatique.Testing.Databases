//-----------------------------------------------------------------------
// <copyright file="DatabaseMigrationTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.DemoApp.DataAccessLayer.Migrations.Tests
{
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using PosInformatique.UnitTests.Databases;
    using PosInformatique.UnitTests.Databases.SqlServer;

    public class DatabaseMigrationTest
    {
        [Fact]
        public async Task MigrationWithConsoleApp()
        {
            const string InitialDatabaseName = "DemoApp_InitialDatabase";
            const string TargetDatabaseName = "DemoApp_TargetDatabase";

            var server = new SqlServer($"Data Source=(localdb)\\DemoApp; Integrated Security=True");

            // Create the initial database
            var initialDatabase = Task.Run(() => server.CreateEmptyDatabase(InitialDatabaseName));

            // Create the target database
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DemoAppDbContext>()
                .UseSqlServer();
            var dbContext = new DemoAppDbContext(dbContextOptionsBuilder.Options);

            var targetDatabase = Task.Run(() => server.CreateDatabaseAsync(TargetDatabaseName, dbContext));

            // Wait both task
            await Task.WhenAll(initialDatabase, targetDatabase);

            // Call the console application to perform migration of the "DemoApp_InitialDatabase"
            var args = new[]
            {
                initialDatabase.Result.ConnectionString,
            };

            await Program.Main(args);

            // Compare the initial and target database
            var comparerOptions = new SqlDatabaseComparerOptions()
            {
                ExcludedTables =
                {
                    { "__EFMigrationsHistory" },
                },
            };

            var differences = await SqlServerDatabaseComparer.CompareAsync(initialDatabase.Result, targetDatabase.Result, comparerOptions);

            differences.IsIdentical.Should().BeTrue(differences.ToString());
        }
    }
}