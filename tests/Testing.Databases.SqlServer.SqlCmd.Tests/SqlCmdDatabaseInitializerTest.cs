//-----------------------------------------------------------------------
// <copyright file="SqlCmdDatabaseInitializerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    [Collection("PosInformatique.Testing.Databases.SqlServer.Tests")]
    public class SqlCmdDatabaseInitializerTest : IClassFixture<SqlServerDatabaseInitializer>
    {
        private const string ConnectionString = $"Data Source=(localDB)\\posinfo-tests; Initial Catalog={nameof(SqlCmdDatabaseInitializerTest)}; Integrated Security=True";

        private readonly SqlServerDatabase database;

        private readonly SqlServerDatabaseInitializer initializer;

        public SqlCmdDatabaseInitializerTest(SqlServerDatabaseInitializer initializer)
        {
            this.initializer = initializer;

            var settings = new SqlCmdRunScriptSettings()
            {
                Variables =
                {
                    { "DatabaseName", nameof(SqlCmdDatabaseInitializerTest) },
                },
            };

            this.database = initializer.Initialize("SqlCmdDatabaseInitializerTest.Script.sql", ConnectionString, settings);

            var table = this.database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().BeEmpty();

            // Insert data to check the connection.
            this.database.InsertInto("MyTable", new { Id = 1, Name = "Name 1" });
            this.database.InsertInto("MyTable", new { Id = 2, Name = "Name 2" });
        }

        [Fact]
        public void Test1()
        {
            this.initializer.IsInitialized.Should().BeTrue();

            var currentUser = this.database.ExecuteQuery("SELECT SUSER_NAME()");
            currentUser.Rows[0][0].Should().Be($"{Environment.UserDomainName}\\{Environment.UserName}");

            // Check the constructor has been called
            var table = this.database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().HaveCount(2);

            table.Rows[0]["Id"].Should().Be(1);
            table.Rows[0]["Name"].Should().Be("Name 1");

            table.Rows[1]["Id"].Should().Be(2);
            table.Rows[1]["Name"].Should().Be("Name 2");

            // Insert a row which should not be use in other tests.
            this.database.InsertInto("MyTable", new { Id = 99, Name = "Should not be here for the next test" });
        }

        [Fact]
        public void Test2()
        {
            this.initializer.IsInitialized.Should().BeTrue();

            var currentUser = this.database.ExecuteQuery("SELECT SUSER_NAME()");
            currentUser.Rows[0][0].Should().Be($"{Environment.UserDomainName}\\{Environment.UserName}");

            // Check the constructor has been called
            var table = this.database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().HaveCount(2);

            table.Rows[0]["Id"].Should().Be(1);
            table.Rows[0]["Name"].Should().Be("Name 1");

            table.Rows[1]["Id"].Should().Be(2);
            table.Rows[1]["Name"].Should().Be("Name 2");

            // Insert a row which should not be use in other tests.
            this.database.InsertInto("MyTable", new { Id = 99, Name = "Should not be here for the next test" });
        }

        [Fact]
        public async Task Test1Async()
        {
            this.initializer.IsInitialized.Should().BeTrue();

            var currentUser = await this.database.ExecuteQueryAsync("SELECT SUSER_NAME()");
            currentUser.Rows[0][0].Should().Be($"{Environment.UserDomainName}\\{Environment.UserName}");

            // Check the constructor has been called
            var table = await this.database.ExecuteQueryAsync("SELECT * FROM MyTable");

            table.Rows.Should().HaveCount(2);

            table.Rows[0]["Id"].Should().Be(1);
            table.Rows[0]["Name"].Should().Be("Name 1");

            table.Rows[1]["Id"].Should().Be(2);
            table.Rows[1]["Name"].Should().Be("Name 2");

            // Insert a row which should not be use in other tests.
            await this.database.InsertIntoAsync("MyTable", new { Id = 99, Name = "Should not be here for the next test" });
        }

        [Fact]
        public async Task Test2Async()
        {
            this.initializer.IsInitialized.Should().BeTrue();

            var currentUser = await this.database.ExecuteQueryAsync("SELECT SUSER_NAME()");
            currentUser.Rows[0][0].Should().Be($"{Environment.UserDomainName}\\{Environment.UserName}");

            // Check the constructor has been called
            var table = await this.database.ExecuteQueryAsync("SELECT * FROM MyTable");

            table.Rows.Should().HaveCount(2);

            table.Rows[0]["Id"].Should().Be(1);
            table.Rows[0]["Name"].Should().Be("Name 1");

            table.Rows[1]["Id"].Should().Be(2);
            table.Rows[1]["Name"].Should().Be("Name 2");

            // Insert a row which should not be use in other tests.
            await this.database.InsertIntoAsync("MyTable", new { Id = 99, Name = "Should not be here for the next test" });
        }
    }
}