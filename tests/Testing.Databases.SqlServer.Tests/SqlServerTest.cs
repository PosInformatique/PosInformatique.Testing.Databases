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
        public async Task CreateAndDelete_WithSpecificName()
        {
            using var temporaryFolder = TemporaryFolder.Create();

            var server = new SqlServer(ConnectionString);

            var settings = new SqlDatabaseCreationSettings()
            {
                DataFileName = Path.Combine(temporaryFolder.Path, "TheSpecificName.mdf"),
            };

            var database = server.CreateEmptyDatabase("CreateAndDeleteDB_WithSpecificName", settings);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=CreateAndDeleteDB_WithSpecificName;Integrated Security=True");

            var table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB_WithSpecificName'");
            table.Rows.Should().HaveCount(1);

            // Check the location of the database
            File.Exists(Path.Combine(temporaryFolder.Path, "TheSpecificName.mdf")).Should().BeTrue();
            File.Exists(Path.Combine(temporaryFolder.Path, "TheSpecificName_log.ldf")).Should().BeTrue();

            var result = database.ExecuteQuery("SELECT * FROM [sys].[database_files] ORDER BY [physical_name]");

            result.Rows.Should().HaveCount(2);

            result.Rows[0]["name"].Should().Be("CreateAndDeleteDB_WithSpecificName");
            result.Rows[0]["physical_name"].Should().Be(Path.Combine(temporaryFolder.Path, "TheSpecificName.mdf"));
            result.Rows[0]["type_desc"].Should().Be("ROWS");

            result.Rows[1]["name"].Should().Be("TheSpecificName_log");
            result.Rows[1]["physical_name"].Should().Be(Path.Combine(temporaryFolder.Path, "TheSpecificName_log.ldf"));
            result.Rows[1]["type_desc"].Should().Be("LOG");

            // Delete the database
            server.DeleteDatabase("CreateAndDeleteDB_WithSpecificName");

            table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB_WithSpecificName'");
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
        public async Task CreateAndDeleteAsync_WithSpecificName()
        {
            using var temporaryFolder = TemporaryFolder.Create();

            var server = new SqlServer(ConnectionString);

            var settings = new SqlDatabaseCreationSettings()
            {
                DataFileName = Path.Combine(temporaryFolder.Path, "TheSpecificNameAsync.mdf"),
            };

            var database = await server.CreateEmptyDatabaseAsync("CreateAndDeleteDB_WithSpecificNameAsync", settings);

            database.ConnectionString.Should().Be("Data Source=(localDB)\\posinfo-tests;Initial Catalog=CreateAndDeleteDB_WithSpecificNameAsync;Integrated Security=True");

            var table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB_WithSpecificNameAsync'");
            table.Rows.Should().HaveCount(1);

            // Check the location of the database
            File.Exists(Path.Combine(temporaryFolder.Path, "TheSpecificNameAsync.mdf")).Should().BeTrue();
            File.Exists(Path.Combine(temporaryFolder.Path, "TheSpecificNameAsync_log.ldf")).Should().BeTrue();

            var result = database.ExecuteQuery("SELECT * FROM [sys].[database_files] ORDER BY [physical_name]");

            result.Rows.Should().HaveCount(2);

            result.Rows[0]["name"].Should().Be("CreateAndDeleteDB_WithSpecificNameAsync");
            result.Rows[0]["physical_name"].Should().Be(Path.Combine(temporaryFolder.Path, "TheSpecificNameAsync.mdf"));
            result.Rows[0]["type_desc"].Should().Be("ROWS");

            result.Rows[1]["name"].Should().Be("TheSpecificNameAsync_log");
            result.Rows[1]["physical_name"].Should().Be(Path.Combine(temporaryFolder.Path, "TheSpecificNameAsync_log.ldf"));
            result.Rows[1]["type_desc"].Should().Be("LOG");

            // Delete the database
            await server.DeleteDatabaseAsync("CreateAndDeleteDB_WithSpecificNameAsync");

            table = await server.Master.ExecuteQueryAsync("SELECT * FROM [sys].[databases] WHERE [name] = 'CreateAndDeleteDB_WithSpecificNameAsync'");
            table.Rows.Should().BeEmpty();
        }

        private sealed class TemporaryFolder : IDisposable
        {
            private TemporaryFolder(string path)
            {
                this.Path = path;
            }

            public string Path { get; }

            public static TemporaryFolder Create()
            {
                var temporaryFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "PosInformatique.Testing.Databases.SqlServer.Tests", Guid.NewGuid().ToString());

                Directory.CreateDirectory(temporaryFolder);

                return new TemporaryFolder(temporaryFolder);
            }

            public void Dispose()
            {
                try
                {
                    Directory.Delete(this.Path, true);
                }
                catch (IOException)
                {
                    // Ignore the errors.
                }
            }
        }
    }
}