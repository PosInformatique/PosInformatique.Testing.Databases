//-----------------------------------------------------------------------
// <copyright file="SqlServerDacExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    [Collection("PosInformatique.Testing.Databases.SqlServer.Tests")]
    public class SqlServerDacExtensionsTest
    {
        private static readonly string ConnectionString = ConnectionStrings.Get();

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void DeployDacPackage(bool withSettings)
        {
            // Create existing database to be sure the database is recreated when deploying the database with a DACPAC
            CreateDatabase("SqlServerDacExtensionsTest_DeployDacPackage");

            var server = new SqlServer(ConnectionString);

            SqlServerDacDeploymentSettings settings = null;

            if (withSettings)
            {
                settings = new SqlServerDacDeploymentSettings();
            }

            var database = server.DeployDacPackage("PosInformatique.Testing.Databases.SqlServer.Tests.DacPac.dacpac", "SqlServerDacExtensionsTest_DeployDacPackage", settings);

            var table = database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().BeEmpty();

            // Insert data to check the connection.
            database.InsertInto("MyTable", new { Id = 1, Name = "Name 1" });
            database.InsertInto("MyTable", new { Id = 2, Name = "Name 2" });
        }

        [Fact]
        public void DeployDacPackage_WithSpecificDataFileName()
        {
            // Create existing database to be sure the database is recreated when deploying the database with a DACPAC
            CreateDatabase("SqlServerDacExtensionsTest_DeployDacPackage_WithSpecificDataFileName");

            using var otherDataPath = OtherDatabasePath.Create();

            var server = new SqlServer(ConnectionString);

            var settings = new SqlServerDacDeploymentSettings()
            {
                DataFileName = Path.Combine(otherDataPath.Path, "TheSpecificDataFileName.mdf"),
            };

            var database = server.DeployDacPackage("PosInformatique.Testing.Databases.SqlServer.Tests.DacPac.dacpac", "SqlServerDacExtensionsTest_DeployDacPackage_WithSpecificDataFileName", settings);

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

            result.Rows[0]["name"].Should().Be("SqlServerDacExtensionsTest_DeployDacPackage_WithSpecificDataFileName");
            result.Rows[0]["physical_name"].Should().Be(Path.Combine(otherDataPath.Path, "TheSpecificDataFileName.mdf"));
            result.Rows[0]["type_desc"].Should().Be("ROWS");

            result.Rows[1]["name"].Should().Be("TheSpecificDataFileName_log");
            result.Rows[1]["physical_name"].Should().Be(Path.Combine(otherDataPath.Path, "TheSpecificDataFileName_log.ldf"));
            result.Rows[1]["type_desc"].Should().Be("LOG");

            // Delete the database (for deleting the temporary folder).
            server.DeleteDatabase("SqlServerDacExtensionsTest_DeployDacPackage_WithSpecificDataFileName");
        }

        [Fact]
        public void DeployDacPackage_WithDatabaseArgumentNull()
        {
            var act = () =>
            {
                SqlServerDacExtensions.DeployDacPackage(null, default, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
               .WithParameterName("server");
        }

        [Fact]
        public void DeployDacPackage_WithFileNameArgumentNull()
        {
            var server = new SqlServer(ConnectionString);

            server.Invoking(s => s.DeployDacPackage(null, default))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("fileName");
        }

        [Fact]
        public void DeployDacPackage_WithDatabaseNameArgumentNull()
        {
            var server = new SqlServer(ConnectionString);

            server.Invoking(s => s.DeployDacPackage("C:/Directory/FileNotFound.sql", null))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("databaseName");
        }

        [Fact]
        public void DeployDacPackage_WithFileNotFound()
        {
            var server = new SqlServer(ConnectionString);

            server.Invoking(s => s.DeployDacPackage("C:/Directory/FileNotFound.sql", "The database"))
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