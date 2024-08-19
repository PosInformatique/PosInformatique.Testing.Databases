//-----------------------------------------------------------------------
// <copyright file="SqlServerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer.Tests
{
    [Collection(nameof(SqlServerTest))]
    public class SqlServerTest
    {
        private const string MasterConnectionString = "Data Source=(localDB)\\posinfo-unit-tests; Initial Catalog=master; Integrated Security=True";

        [Fact]
        public void CreateEmptyDatabase()
        {
            var server = new SqlServer(MasterConnectionString);

            server.DeleteDatabase("SqlServerTest");

            var database = server.CreateEmptyDatabase("SqlServerTest");

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-unit-tests;Initial Catalog=SqlServerTest;Integrated Security=True");

            var table = database.ExecuteQuery("SELECT * FROM sys.tables");

            table.Rows.Count.Should().Be(0);
        }

        [Fact]
        public void DeleteDatabase()
        {
            var master = new SqlServerDatabase(MasterConnectionString);

            var server = new SqlServer(MasterConnectionString);

            server.DeleteDatabase("SqlServerTest");
            server.CreateEmptyDatabase("SqlServerTest");

            server.DeleteDatabase("SqlServerTest");

            var table = master.ExecuteQuery("SELECT * FROM sys.databases WHERE name = 'SqlServerTest'");

            table.Rows.Count.Should().Be(0);
        }

        [Fact]
        public void GetDatabase()
        {
            var server = new SqlServer(MasterConnectionString);

            server.CreateEmptyDatabase("SqlServerTest");

            var database = server.GetDatabase("SqlServerTest");

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-unit-tests;Initial Catalog=SqlServerTest;Integrated Security=True");

            var table = database.ExecuteQuery("SELECT * FROM sys.tables");

            table.Rows.Count.Should().Be(0);
        }
    }
}