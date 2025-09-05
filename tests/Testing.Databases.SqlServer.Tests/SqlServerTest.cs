//-----------------------------------------------------------------------
// <copyright file="SqlServerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    [Collection("PosInformatique.Testing.Databases.SqlServer.Tests")]
    public class SqlServerTest
    {
        private const string ConnectionString = $"Data Source=(localDB)\\posinfo-tests; Initial Catalog={nameof(SqlServerTest)}; Integrated Security=True";

        [Theory]
        [InlineData("Data Source=TheServer; Initial Catalog=TheDB; User ID=TheID; Password=ThePassword", "Data Source=TheServer;Initial Catalog=master;User ID=TheID;Password=ThePassword")]
        [InlineData("Data Source=TheServer; Initial Catalog=TheDB; Integrated Security=True", "Data Source=TheServer;Initial Catalog=master;Integrated Security=True")]
        public void Constructor(string connectionString, string expectedMasterConnectionString)
        {
            var server = new SqlServer(connectionString);

            server.Master.ConnectionString.Should().Be(expectedMasterConnectionString);
            server.Master.Server.Should().BeSameAs(server);
        }

        [Fact]
        public async Task CreateAndDeleteAsync()
        {
            var server = new SqlServer(ConnectionString);

            var database = await server.CreateEmptyDatabaseAsync("CreateAndDeleteDB", CancellationToken.None);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=CreateAndDeleteDB;Integrated Security=True");

            var table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB'");
            table.Rows.Should().HaveCount(1);

            // Delete the database
            await server.DeleteDatabaseAsync("CreateAndDeleteDB", CancellationToken.None);

            table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB'");
            table.Rows.Should().BeEmpty();
        }
    }
}