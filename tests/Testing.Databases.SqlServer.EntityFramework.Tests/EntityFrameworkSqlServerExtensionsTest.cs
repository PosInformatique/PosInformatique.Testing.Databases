//-----------------------------------------------------------------------
// <copyright file="EntityFrameworkSqlServerExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    using Microsoft.EntityFrameworkCore;

    [Collection("PosInformatique.Testing.Databases.SqlServer.Tests")]
    public class EntityFrameworkSqlServerExtensionsTest
    {
        private const string ConnectionString = $"Data Source=(localDB)\\posinfo-tests; Integrated Security=True";

        [Fact]
        public async Task Create_WithNoExistingDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContextTest>()
                .UseSqlServer("Data Source=OtherServer;");

            using var dbContext = new DbContextTest(optionsBuilder.Options);

            var server = new SqlServer(ConnectionString);
            server.DeleteDatabase(nameof(EntityFrameworkSqlServerExtensionsTest));

            var database = server.CreateDatabase(nameof(EntityFrameworkSqlServerExtensionsTest), dbContext);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=EntityFrameworkSqlServerExtensionsTest;Integrated Security=True");

            var tables = await database.GetTablesAsync();

            tables.Should().HaveCount(1);

            tables[0].Name.Should().Be("Entity");

            tables[0].Columns.Should().HaveCount(2);
            tables[0].Columns[0].Name.Should().Be("Id");
            tables[0].Columns[1].Name.Should().Be("Name");
        }

        [Fact]
        public async Task Create_WithAlreadyExistingDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContextTest>()
                .UseSqlServer("Data Source=OtherServer;");

            using var dbContext = new DbContextTest(optionsBuilder.Options);

            var server = new SqlServer(ConnectionString);
            var emptyDatabase = server.CreateEmptyDatabase(nameof(EntityFrameworkSqlServerExtensionsTest));

            emptyDatabase.ExecuteNonQuery("CREATE TABLE [MustBeDeleted] ([Id] INT)");

            var database = server.CreateDatabase(nameof(EntityFrameworkSqlServerExtensionsTest), dbContext);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=EntityFrameworkSqlServerExtensionsTest;Integrated Security=True");

            var tables = await database.GetTablesAsync();

            tables.Should().HaveCount(1);

            tables[0].Name.Should().Be("Entity");

            tables[0].Columns.Should().HaveCount(2);
            tables[0].Columns[0].Name.Should().Be("Id");
            tables[0].Columns[1].Name.Should().Be("Name");
        }

        [Fact]
        public async Task CreateAsync_WithNoExistingDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContextTest>()
                .UseSqlServer("Data Source=OtherServer;");

            using var dbContext = new DbContextTest(optionsBuilder.Options);

            var server = new SqlServer(ConnectionString);
            await server.DeleteDatabaseAsync(nameof(EntityFrameworkSqlServerExtensionsTest));

            var database = await server.CreateDatabaseAsync(nameof(EntityFrameworkSqlServerExtensionsTest), dbContext);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=EntityFrameworkSqlServerExtensionsTest;Integrated Security=True");

            var tables = await database.GetTablesAsync();

            tables.Should().HaveCount(1);

            tables[0].Name.Should().Be("Entity");

            tables[0].Columns.Should().HaveCount(2);
            tables[0].Columns[0].Name.Should().Be("Id");
            tables[0].Columns[1].Name.Should().Be("Name");
        }

        [Fact]
        public async Task CreateAsync_WithAlreadyExistingDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContextTest>()
                .UseSqlServer("Data Source=OtherServer;");

            using var dbContext = new DbContextTest(optionsBuilder.Options);

            var server = new SqlServer(ConnectionString);
            var emptyDatabase = await server.CreateEmptyDatabaseAsync(nameof(EntityFrameworkSqlServerExtensionsTest));

            await emptyDatabase.ExecuteNonQueryAsync("CREATE TABLE [MustBeDeleted] ([Id] INT)");

            var database = await server.CreateDatabaseAsync(nameof(EntityFrameworkSqlServerExtensionsTest), dbContext);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=EntityFrameworkSqlServerExtensionsTest;Integrated Security=True");

            var tables = await database.GetTablesAsync();

            tables.Should().HaveCount(1);

            tables[0].Name.Should().Be("Entity");

            tables[0].Columns.Should().HaveCount(2);
            tables[0].Columns[0].Name.Should().Be("Id");
            tables[0].Columns[1].Name.Should().Be("Name");
        }

        private sealed class DbContextTest : DbContext
        {
            public DbContextTest(DbContextOptions<DbContextTest> options)
                : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Entity>();
            }
        }

        private sealed class Entity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}