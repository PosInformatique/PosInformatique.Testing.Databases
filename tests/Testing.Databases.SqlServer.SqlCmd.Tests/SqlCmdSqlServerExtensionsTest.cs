//-----------------------------------------------------------------------
// <copyright file="SqlCmdSqlServerExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    [Collection("PosInformatique.Testing.Databases.SqlServer.Tests")]
    public class SqlCmdSqlServerExtensionsTest
    {
        private const string ConnectionString = $"Data Source=(localDB)\\posinfo-tests; Integrated Security=True";

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void RunScript(bool withSettings)
        {
            var server = new SqlServer(ConnectionString);

            server.DeleteDatabase("SqlCmdSqlServerExtensionsTest_RunScript");

            SqlCmdRunScriptSettings settings = null;

            if (withSettings)
            {
                settings = new SqlCmdRunScriptSettings();
            }

            using var temporaryFile = TemporaryFile.Create();

            File.WriteAllText(
                temporaryFile.FileName,
                """
                PRINT 'GOOOOOO !'
                GO
                CREATE DATABASE SqlCmdSqlServerExtensionsTest_RunScript
                GO
                USE SqlCmdSqlServerExtensionsTest_RunScript
                GO
                CREATE TABLE MyTable (Name VARCHAR(50))
                """);

            server.Master.RunScript(temporaryFile.FileName, settings);

            var database = server.GetDatabase("SqlCmdSqlServerExtensionsTest_RunScript");

            var table = database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().BeEmpty();
        }

        [Fact]
        public void RunScript_WithVariables()
        {
            var server = new SqlServer(ConnectionString);

            server.DeleteDatabase("SqlCmdSqlServerExtensionsTest_RunScript_WithVariables");

            var settings = new SqlCmdRunScriptSettings()
            {
                Variables =
                {
                    { "DatabaseName", "SqlCmdSqlServerExtensionsTest_RunScript_WithVariables" },
                    { "TableName", "MyTable" },
                },
            };

            using var temporaryFile = TemporaryFile.Create();

            File.WriteAllText(
                temporaryFile.FileName,
                """
                PRINT 'GOOOOOO !'
                GO
                CREATE DATABASE [$(DatabaseName)]
                GO
                USE [$(DatabaseName)]
                GO
                CREATE TABLE [$(TableName)] (Name VARCHAR(50))
                """);

            server.Master.RunScript(temporaryFile.FileName, settings);

            var database = server.GetDatabase("SqlCmdSqlServerExtensionsTest_RunScript");

            var table = database.ExecuteQuery("SELECT * FROM MyTable");

            table.Rows.Should().BeEmpty();
        }

        [Fact]
        public void RunScript_WithErros()
        {
            var server = new SqlServer(ConnectionString);

            server.DeleteDatabase("SqlCmdSqlServerExtensionsTest_RunScript_WithErros");

            var settings = new SqlCmdRunScriptSettings()
            {
                Variables =
                {
                    { "DatabaseName", "SqlCmdSqlServerExtensionsTest_RunScript_WithErros" },
                    { "TableName", "MyTable" },
                },
            };

            using var temporaryFile = TemporaryFile.Create();

            File.WriteAllText(
                temporaryFile.FileName,
                """
                PRINT 'GOOOOOO !'
                GO
                CREATE DATABASE [$(DatabaseName)]
                GO
                USE [$(DatabaseName)]
                GO
                CREATE TABLE ErrorBlabla
                """);

            server.Master.Invoking(m => m.RunScript(temporaryFile.FileName, settings))
                .Should().ThrowExactly<SqlCmdException>()
                .Which.Output.Should().StartWith(
                """
                GOOOOOO !
                Changed database context to 'SqlCmdSqlServerExtensionsTest_RunScript_WithErros'.
                Msg 102, Level 15, State 1,
                """)
                .And.EndWith("Incorrect syntax near 'ErrorBlabla'.");

            var database = server.GetDatabase("SqlCmdSqlServerExtensionsTest_RunScript_WithErros");

            var table = database.ExecuteQuery("SELECT * FROM sys.tables");

            table.Rows.Should().BeEmpty();
        }

        [Fact]
        public void RunScript_WithDatabaseArgumentNull()
        {
            var act = () =>
            {
                SqlCmdSqlServerDatabaseExtensions.RunScript(null, default, default);
            };

            act.Should().ThrowExactly<ArgumentNullException>()
               .WithParameterName("database");
        }

        [Fact]
        public void RunScript_WithFileNameArgumentNull()
        {
            var server = new SqlServer(ConnectionString);

            server.Master.Invoking(m => m.RunScript(null, default))
                .Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("fileName");
        }

        [Fact]
        public void RunScript_WithFileNotFound()
        {
            var server = new SqlServer(ConnectionString);

            server.Master.Invoking(m => m.RunScript("C:/Directory/FileNotFound.sql", default))
                .Should().ThrowExactly<FileNotFoundException>()
                .WithMessage("Could not find file 'C:/Directory/FileNotFound.sql'")
                .Which.FileName.Should().Be("C:/Directory/FileNotFound.sql");
        }
    }
}