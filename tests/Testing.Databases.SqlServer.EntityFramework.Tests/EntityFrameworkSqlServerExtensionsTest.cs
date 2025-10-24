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
        private static readonly string ConnectionString = ConnectionStrings.Get();

        [Fact]
        public async Task Create_WithNoExistingDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContextTest>()
                .UseSqlServer("Data Source=OtherServer;");

            using var dbContext = new DbContextTest(optionsBuilder.Options);

            var server = new SqlServer(ConnectionString);
            server.DeleteDatabase(nameof(EntityFrameworkSqlServerExtensionsTest));

            var database = server.CreateDatabase(nameof(EntityFrameworkSqlServerExtensionsTest), dbContext);

            database.ConnectionString.Should().Be(ConnectionStrings.Get("EntityFrameworkSqlServerExtensionsTest"));

            var tables = await database.GetTablesAsync(TestContext.Current.CancellationToken);

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

            database.ConnectionString.Should().Be(ConnectionStrings.Get("EntityFrameworkSqlServerExtensionsTest"));

            var tables = await database.GetTablesAsync(TestContext.Current.CancellationToken);

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
            await server.DeleteDatabaseAsync(nameof(EntityFrameworkSqlServerExtensionsTest), TestContext.Current.CancellationToken);

            var database = await server.CreateDatabaseAsync(nameof(EntityFrameworkSqlServerExtensionsTest), dbContext);

            database.ConnectionString.Should().Be(ConnectionStrings.Get("EntityFrameworkSqlServerExtensionsTest"));

            var tables = await database.GetTablesAsync(TestContext.Current.CancellationToken);

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
            var emptyDatabase = await server.CreateEmptyDatabaseAsync(nameof(EntityFrameworkSqlServerExtensionsTest), default, TestContext.Current.CancellationToken);

            await emptyDatabase.ExecuteNonQueryAsync("CREATE TABLE [MustBeDeleted] ([Id] INT)", TestContext.Current.CancellationToken);

            var database = await server.CreateDatabaseAsync(nameof(EntityFrameworkSqlServerExtensionsTest), dbContext);

            database.ConnectionString.Should().Be(ConnectionStrings.Get("EntityFrameworkSqlServerExtensionsTest"));

            var tables = await database.GetTablesAsync(TestContext.Current.CancellationToken);

            tables.Should().HaveCount(1);

            tables[0].Name.Should().Be("Entity");

            tables[0].Columns.Should().HaveCount(2);
            tables[0].Columns[0].Name.Should().Be("Id");
            tables[0].Columns[1].Name.Should().Be("Name");
        }

        [Fact]
        public void CreateDatabase_WithServerNull()
        {
            var act = () =>
            {
                EntityFrameworkSqlServerExtensions.CreateDatabase(null, default, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
               .WithParameterName("server");
        }

        [Fact]
        public void CreateDatabase_WithNameArgumentNull()
        {
            var server = new SqlServer(ConnectionString);

            server.Invoking(s => s.CreateDatabase(null, default))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void CreateDatabase_WithContextArgumentNull()
        {
            var server = new SqlServer(ConnectionString);

            server.Invoking(s => s.CreateDatabase("The name", default))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("context");
        }

        [Fact]
        public void CreateDatabaseAsync_WithServerNull()
        {
            var act = async () =>
            {
                await EntityFrameworkSqlServerExtensions.CreateDatabaseAsync(null, default, default);
            };

            act.Should().ThrowExactlyAsync<ArgumentNullException>()
               .WithParameterName("server");
        }

        [Fact]
        public void CreateDatabaseAsync_WithNameArgumentNull()
        {
            var server = new SqlServer(ConnectionString);

            server.Invoking(s => s.CreateDatabaseAsync(null, default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("name");
        }

        [Fact]
        public void CreateDatabaseAsync_WithContextArgumentNull()
        {
            var server = new SqlServer(ConnectionString);

            server.Invoking(s => s.CreateDatabaseAsync("The name", default))
                .Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("context");
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