//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer.Tests
{
    [Collection("PosInformatique.UnitTests.Databases.SqlServer.Tests")]
    public class SqlServerDatabaseExtensionsTest
    {
        private const string ConnectionString = $"Data Source=(localDB)\\posinfo-unit-tests; Initial Catalog={nameof(SqlServerDatabaseExtensionsTest)}; Integrated Security=True";

        [Fact]
        public void AsAdministrator()
        {
            var server = new SqlServer(ConnectionString);

            var database = server.GetDatabase("TheDatabase");

            database.AsAdministrator().ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-unit-tests;Initial Catalog=TheDatabase;Integrated Security=True");
        }

        [Fact]
        public void InsertInto_EnableIdentity()
        {
            var server = new SqlServer(ConnectionString);

            var database = server.CreateEmptyDatabase("SqlServerDatabaseExtensionsTest");

            database.AsAdministrator().ExecuteNonQuery(@"
                CREATE TABLE TableTest
                (
                    Id          INT             NOT NULL IDENTITY(1, 1),
                    Name        VARCHAR(50)     NOT NULL,
                    Boolean     BIT             NOT NULL,
                    BooleanNull BIT             NULL,
                    Binary      VARBINARY(MAX)  NULL,
                )");

            database.InsertInto(
                "TableTest",
                false,
                new { Name = "Name 1", Boolean = true, BooleanNull = (bool?)true, Binary = new byte[] { 1, 2 } },
                new { Name = "Name 2", Boolean = false, BooleanNull = (bool?)null, Binary = (byte[])null });

            var table = database.ExecuteQuery("SELECT * FROM [TableTest] ORDER BY [Id]");

            table.Rows.Should().HaveCount(2);

            table.Rows[0]["Id"].Should().Be(1);
            table.Rows[0]["Name"].Should().Be("Name 1");
            table.Rows[0]["Binary"].As<byte[]>().Should().Equal([1, 2]);
            table.Rows[0]["Boolean"].As<bool>().Should().Be(true);
            table.Rows[0]["BooleanNull"].As<bool>().Should().Be(true);

            table.Rows[1]["Id"].Should().Be(2);
            table.Rows[1]["Name"].Should().Be("Name 2");
            table.Rows[1]["Binary"].Should().Be(DBNull.Value);
            table.Rows[1]["Boolean"].As<bool>().Should().Be(false);
            table.Rows[1]["BooleanNull"].Should().Be(DBNull.Value);
        }

        [Fact]
        public void InsertInto_DisableIdentity()
        {
            var server = new SqlServer(ConnectionString);

            var database = server.CreateEmptyDatabase("SqlServerDatabaseExtensionsTest");

            database.AsAdministrator().ExecuteNonQuery(@"
                CREATE TABLE TableTest
                (
                    Id      INT             NOT NULL IDENTITY(1, 1),
                    Name    VARCHAR(50)     NOT NULL,
                    Boolean     BIT         NOT NULL,
                    BooleanNull BIT         NULL,
                    Binary  VARBINARY(MAX)  NULL,
                )");

            database.InsertInto(
                "TableTest",
                true,
                new { Id = 10, Name = "Name 1", Boolean = true, BooleanNull = (bool?)true, Binary = new byte[] { 1, 2 } },
                new { Id = 20, Name = "Name 2", Boolean = false, BooleanNull = (bool?)null, Binary = (byte[])null });

            var table = database.ExecuteQuery("SELECT * FROM [TableTest] ORDER BY [Id]");

            table.Rows.Should().HaveCount(2);

            table.Rows[0]["Id"].Should().Be(10);
            table.Rows[0]["Name"].Should().Be("Name 1");
            table.Rows[0]["Binary"].As<byte[]>().Should().Equal([1, 2]);
            table.Rows[0]["Boolean"].As<bool>().Should().Be(true);
            table.Rows[0]["BooleanNull"].As<bool>().Should().Be(true);

            table.Rows[1]["Id"].Should().Be(20);
            table.Rows[1]["Name"].Should().Be("Name 2");
            table.Rows[1]["Binary"].Should().Be(DBNull.Value);
            table.Rows[1]["Boolean"].As<bool>().Should().Be(false);
            table.Rows[1]["BooleanNull"].Should().Be(DBNull.Value);
        }
    }
}