//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseInitializerDbContextWithLoginTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer.Tests
{
    using Microsoft.EntityFrameworkCore;

    [Collection("PosInformatique.UnitTests.Databases.SqlServer.Tests")]
    public class SqlServerDatabaseInitializerDbContextWithLoginTest : IClassFixture<SqlServerDatabaseInitializer>
    {
        private const string ConnectionString = $"Data Source=(localDB)\\posinfo-unit-tests; Initial Catalog={nameof(SqlServerDatabaseInitializerDbContextWithLoginTest)}; User ID=ServiceAccountLogin; Password=P@ssw0rd";

        private readonly SqlServerDatabase database;

        public SqlServerDatabaseInitializerDbContextWithLoginTest(SqlServerDatabaseInitializer initializer)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContextTest>()
                .UseSqlServer(ConnectionString);

            using var context = new DbContextTest(optionsBuilder.Options);

            this.database = initializer.Initialize(context);

            this.database.AsAdministrator().ExecuteNonQuery("IF NOT EXISTS (SELECT 1 FROM [sys].[sysusers] WHERE name = 'ServiceAccountUser') CREATE USER [ServiceAccountUser] FOR LOGIN [ServiceAccountLogin]");
            this.database.AsAdministrator().ExecuteNonQuery("GRANT CONNECT TO [ServiceAccountUser]");
            this.database.AsAdministrator().ExecuteNonQuery("GRANT SELECT, INSERT ON [MyTable] TO [ServiceAccountUser]");

            var table = this.database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().BeEmpty();

            this.database.InsertInto("MyTable", new { Id = 1, Name = "Name 1" });
            this.database.InsertInto("MyTable", new { Id = 2, Name = "Name 2" });
        }

        [Fact]
        public void Test1()
        {
            var currentUser = this.database.ExecuteQuery("SELECT SUSER_NAME()");
            currentUser.Rows[0][0].Should().Be("ServiceAccountLogin");

            // Check the constructor has been called
            var table = this.database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().HaveCount(2);

            table.Rows[0]["Id"].Should().Be(1);
            table.Rows[0]["Name"].Should().Be("Name 1");

            table.Rows[1]["Id"].Should().Be(2);
            table.Rows[1]["Name"].Should().Be("Name 2");

            // Insert a row which should not be use in other tests.
            this.database.InsertInto("MyTable", new { Id = 99, Name = "Should not be here for the next unit test" });
        }

        [Fact]
        public void Test2()
        {
            var currentUser = this.database.ExecuteQuery("SELECT SUSER_NAME()");
            currentUser.Rows[0][0].Should().Be("ServiceAccountLogin");

            // Check the constructor has been called
            var table = this.database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().HaveCount(2);

            table.Rows[0]["Id"].Should().Be(1);
            table.Rows[0]["Name"].Should().Be("Name 1");

            table.Rows[1]["Id"].Should().Be(2);
            table.Rows[1]["Name"].Should().Be("Name 2");

            // Insert a row which should not be use in other tests.
            this.database.InsertInto("MyTable", new { Id = 99, Name = "Should not be here for the next unit test" });
        }

        private sealed class DbContextTest : DbContext
        {
            public DbContextTest(DbContextOptions<DbContextTest> options)
                : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Table>(t =>
                {
                    t.ToTable("MyTable");

                    t.Property(t => t.Id)
                        .ValueGeneratedNever();

                    t.Property(t => t.Name)
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);
                });
            }
        }

        private sealed class Table
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}