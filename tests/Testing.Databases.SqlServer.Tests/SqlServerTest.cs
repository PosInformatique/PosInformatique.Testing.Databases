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

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task CreateAndDelete(bool withCreationSettings)
        {
            var server = new SqlServer(ConnectionString);

            SqlDatabaseCreationSettings settings = null;

            if (withCreationSettings)
            {
                settings = new SqlDatabaseCreationSettings();
            }

            var database = server.CreateEmptyDatabase("CreateAndDeleteDB", settings);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=CreateAndDeleteDB;Integrated Security=True");

            var table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB'");
            table.Rows.Should().HaveCount(1);

            // Delete the database
            server.DeleteDatabase("CreateAndDeleteDB");

            table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB'");
            table.Rows.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAndDelete_WithSpecificDataFileName()
        {
            using var temporaryFolder = TemporaryFolder.Create();

            var server = new SqlServer(ConnectionString);

            var settings = new SqlDatabaseCreationSettings()
            {
                DataFileName = Path.Combine(temporaryFolder.Path, "TheSpecificDataFileName.mdf"),
            };

            var database = server.CreateEmptyDatabase("CreateAndDeleteDB_WithSpecificDataFileName", settings);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=CreateAndDeleteDB_WithSpecificDataFileName;Integrated Security=True");

            var table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB_WithSpecificDataFileName'");
            table.Rows.Should().HaveCount(1);

            // Check the location of the database
            File.Exists(Path.Combine(temporaryFolder.Path, "TheSpecificDataFileName.mdf")).Should().BeTrue();
            File.Exists(Path.Combine(temporaryFolder.Path, "TheSpecificDataFileName_log.ldf")).Should().BeTrue();

            var result = database.ExecuteQuery("SELECT * FROM [sys].[database_files] ORDER BY [physical_name]");

            result.Rows.Should().HaveCount(2);

            result.Rows[0]["name"].Should().Be("CreateAndDeleteDB_WithSpecificDataFileName");
            result.Rows[0]["physical_name"].Should().Be(Path.Combine(temporaryFolder.Path, "TheSpecificDataFileName.mdf"));
            result.Rows[0]["type_desc"].Should().Be("ROWS");

            result.Rows[1]["name"].Should().Be("TheSpecificDataFileName_log");
            result.Rows[1]["physical_name"].Should().Be(Path.Combine(temporaryFolder.Path, "TheSpecificDataFileName_log.ldf"));
            result.Rows[1]["type_desc"].Should().Be("LOG");

            // Delete the database
            server.DeleteDatabase("CreateAndDeleteDB_WithSpecificDataFileName");

            table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB_WithSpecificDataFileName'");
            table.Rows.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAndDeleteAsync()
        {
            var server = new SqlServer(ConnectionString);

            var database = await server.CreateEmptyDatabaseAsync("CreateAndDeleteDBAsync", new SqlDatabaseCreationSettings(), CancellationToken.None);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=CreateAndDeleteDBAsync;Integrated Security=True");

            var table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDBAsync'");
            table.Rows.Should().HaveCount(1);

            // Delete the database
            await server.DeleteDatabaseAsync("CreateAndDeleteDBAsync", CancellationToken.None);

            table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDBAsync'");
            table.Rows.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAndDeleteAsync_WithSpecificDataFileName()
        {
            using var temporaryFolder = TemporaryFolder.Create();

            var server = new SqlServer(ConnectionString);

            var settings = new SqlDatabaseCreationSettings()
            {
                DataFileName = Path.Combine(temporaryFolder.Path, "TheSpecificDataFileNameAsync.mdf"),
            };

            var database = await server.CreateEmptyDatabaseAsync("CreateAndDeleteDB_WithSpecificDataFileNameAsync", settings);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=CreateAndDeleteDB_WithSpecificDataFileNameAsync;Integrated Security=True");

            var table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB_WithSpecificDataFileNameAsync'");
            table.Rows.Should().HaveCount(1);

            // Check the location of the database
            File.Exists(Path.Combine(temporaryFolder.Path, "TheSpecificDataFileNameAsync.mdf")).Should().BeTrue();
            File.Exists(Path.Combine(temporaryFolder.Path, "TheSpecificDataFileNameAsync_log.ldf")).Should().BeTrue();

            var result = database.ExecuteQuery("SELECT * FROM [sys].[database_files] ORDER BY [physical_name]");

            result.Rows.Should().HaveCount(2);

            result.Rows[0]["name"].Should().Be("CreateAndDeleteDB_WithSpecificDataFileNameAsync");
            result.Rows[0]["physical_name"].Should().Be(Path.Combine(temporaryFolder.Path, "TheSpecificDataFileNameAsync.mdf"));
            result.Rows[0]["type_desc"].Should().Be("ROWS");

            result.Rows[1]["name"].Should().Be("TheSpecificDataFileNameAsync_log");
            result.Rows[1]["physical_name"].Should().Be(Path.Combine(temporaryFolder.Path, "TheSpecificDataFileNameAsync_log.ldf"));
            result.Rows[1]["type_desc"].Should().Be("LOG");

            // Delete the database
            await server.DeleteDatabaseAsync("CreateAndDeleteDB_WithSpecificDataFileNameAsync");

            table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB_WithSpecificDataFileNameAsync'");
            table.Rows.Should().BeEmpty();
        }
    }
}