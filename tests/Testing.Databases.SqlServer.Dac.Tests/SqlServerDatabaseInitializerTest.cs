//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseInitializerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    [Collection("PosInformatique.Testing.Databases.SqlServer.Tests")]
    public class SqlServerDatabaseInitializerTest : IClassFixture<SqlServerDatabaseInitializer>
    {
        private static readonly string ConnectionString = ConnectionStrings.Get(nameof(SqlServerDatabaseInitializerTest));

        private readonly SqlServerDatabase database;

        private readonly SqlServerDatabaseInitializer initializer;

        public SqlServerDatabaseInitializerTest(SqlServerDatabaseInitializer initializer)
        {
            this.initializer = initializer;
            this.database = initializer.Initialize("PosInformatique.Testing.Databases.SqlServer.Tests.DacPac.dacpac", ConnectionString);

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
            currentUser.Rows[0][0].Should().Be(ConnectionStrings.ExtractUserName(ConnectionString));

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
            currentUser.Rows[0][0].Should().Be(ConnectionStrings.ExtractUserName(ConnectionString));

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
            currentUser.Rows[0][0].Should().Be(ConnectionStrings.ExtractUserName(ConnectionString));

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
            currentUser.Rows[0][0].Should().Be(ConnectionStrings.ExtractUserName(ConnectionString));

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
        public void Initialize_WithSpecificDataFileName()
        {
            this.initializer.IsInitialized.Should().BeTrue();

            // Create existing database to be sure the database is recreated when deploying the database with a DACPAC
            CreateDatabase("SqlServerDatabaseInitializerTest_Initialize_WithSpecificDataFileName");

            using var otherDataPath = OtherDatabasePath.Create();

            var server = new SqlServer(ConnectionString);

            var settings = new SqlServerDacDeploymentSettings()
            {
                DataFileName = Path.Combine(otherDataPath.Path, "TheSpecificDataFileName.mdf"),
            };

            var database = server.DeployDacPackage("PosInformatique.Testing.Databases.SqlServer.Tests.DacPac.dacpac", "SqlServerDatabaseInitializerTest_Initialize_WithSpecificDataFileName", settings);

            var table = database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().BeEmpty();

            // Insert data to check the connection.
            database.InsertInto("MyTable", new { Id = 1, Name = "Name 1" });
            database.InsertInto("MyTable", new { Id = 2, Name = "Name 2" });

            // Check the location of the database
            File.Exists(Path.Combine(otherDataPath.Path, "TheSpecificDataFileName.mdf")).Should().BeTrue();
            File.Exists(Path.Combine(otherDataPath.Path, "TheSpecificDataFileName_log.ldf")).Should().BeTrue();

            var result = database.ExecuteQuery("SELECT * FROM [sys].[database_files] ORDER BY [physical_name]");

            result.Rows.Should().HaveCount(2);

            result.Rows[0]["name"].Should().Be("SqlServerDatabaseInitializerTest_Initialize_WithSpecificDataFileName");
            result.Rows[0]["physical_name"].Should().Be(Path.Combine(otherDataPath.Path, "TheSpecificDataFileName.mdf"));
            result.Rows[0]["type_desc"].Should().Be("ROWS");

            result.Rows[1]["name"].Should().Be("SqlServerDatabaseInitializerTest_Initialize_WithSpecificDataFileName_log");
            result.Rows[1]["physical_name"].Should().Be(Path.Combine(otherDataPath.Path, "TheSpecificDataFileName_log.ldf"));
            result.Rows[1]["type_desc"].Should().Be("LOG");

            // Delete the database (for deleting the temporary folder).
            server.DeleteDatabase("SqlServerDatabaseInitializerTest_Initialize_WithSpecificDataFileName");
        }

        [Fact]
        public void Initialize_WithInitializerArgumentNull()
        {
            var act = () =>
            {
                SqlServerDacDatabaseInitializer.Initialize(null, default, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
               .WithParameterName("initializer");
        }

        [Fact]
        public void Initialize_WithFileNameArgumentNull()
        {
            var initializer = new SqlServerDatabaseInitializer();

            initializer.Invoking(i => i.Initialize(null, default))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("packageName");
        }

        [Fact]
        public void Initialize_WithConnectionStringArgumentNull()
        {
            var initializer = new SqlServerDatabaseInitializer();

            initializer.Invoking(i => i.Initialize("The file name", null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("connectionString");
        }

        [Fact]
        public void Initialize_WithPackageNotFound()
        {
            var initializer = new SqlServerDatabaseInitializer();

            initializer.Invoking(i => i.Initialize("C:/Directory/FileNotFound.sql", "The connection stirng"))
                .Should().ThrowExactly<FileNotFoundException>()
                .WithMessage("Could not find file 'C:/Directory/FileNotFound.sql'")
                .Which.FileName.Should().Be("C:/Directory/FileNotFound.sql");
        }

        private static void CreateDatabase(string name)
        {
            var server = new SqlServer(ConnectionString);

            var database = server.CreateEmptyDatabase(name);

            database.ExecuteNonQuery("CREATE TABLE OtherTable (Id INT)");
        }
    }
}