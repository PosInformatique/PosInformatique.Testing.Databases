﻿//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseInitializerDacPacTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer.Tests
{
    [Collection("PosInformatique.UnitTests.Databases.SqlServer.Tests")]
    public class SqlServerDatabaseInitializerDacPacTest : IClassFixture<SqlServerDatabaseInitializer>
    {
        private const string ConnectionString = $"Data Source=(localDB)\\posinfo-unit-tests; Initial Catalog={nameof(SqlServerDatabaseInitializerDacPacTest)}; Integrated Security=True";

        private readonly SqlServerDatabase database;

        public SqlServerDatabaseInitializerDacPacTest(SqlServerDatabaseInitializer initializer)
        {
            this.database = initializer.Initialize("UnitTests.Databases.SqlServer.Tests.DacPac.dacpac", ConnectionString);

            var table = this.database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().BeEmpty();

            this.database.InsertInto("MyTable", new { Id = 1, Name = "Name 1" });
            this.database.InsertInto("MyTable", new { Id = 2, Name = "Name 2" });
        }

        [Fact]
        public void Test1()
        {
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
            this.database.InsertInto("MyTable", new { Id = 99, Name = "Should not be here for the next unit test" });
        }

        [Fact]
        public void Test2()
        {
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
            this.database.InsertInto("MyTable", new { Id = 99, Name = "Should not be here for the next unit test" });
        }
    }
}