//-----------------------------------------------------------------------
// <copyright file="SqlCmdCommandLineArgumentsBuilderTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    using Microsoft.Data.SqlClient;

    public class SqlCmdCommandLineArgumentsBuilderTest
    {
        [Fact]
        public void Constructor()
        {
            var connectionString = new SqlConnectionStringBuilder()
            {
                DataSource = "The data source",
                InitialCatalog = "The initial catalog",
                IntegratedSecurity = true,
                Password = "The password",
                UserID = "The login",
            };

            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(connectionString);

            argumentsBuilder.Database.Should().Be("The initial catalog");
            argumentsBuilder.InputFile.Should().BeNull();
            argumentsBuilder.LoginId.Should().Be("The login");
            argumentsBuilder.Password.Should().Be("The password");
            argumentsBuilder.Server.Should().Be("The data source");
            argumentsBuilder.TrustedConnection.Should().BeTrue();
            argumentsBuilder.Variables.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_WithEmptyConnnectionString()
        {
            var connectionString = new SqlConnectionStringBuilder();

            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(connectionString);

            argumentsBuilder.Database.Should().BeEmpty();
            argumentsBuilder.InputFile.Should().BeNull();
            argumentsBuilder.LoginId.Should().BeEmpty();
            argumentsBuilder.Password.Should().BeEmpty();
            argumentsBuilder.Server.Should().BeEmpty();
            argumentsBuilder.TrustedConnection.Should().BeFalse();
            argumentsBuilder.Variables.Should().BeEmpty();
        }

        [Fact]
        public void Database_ValueChanged()
        {
            var connectionString = new SqlConnectionStringBuilder();

            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(connectionString);

            argumentsBuilder.Database = "The database";

            argumentsBuilder.Database.Should().Be("The database");
        }

        [Fact]
        public void InputFile_ValueChanged()
        {
            var connectionString = new SqlConnectionStringBuilder();

            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(connectionString);

            argumentsBuilder.InputFile = "The input file";

            argumentsBuilder.InputFile.Should().Be("The input file");
        }

        [Fact]
        public void Password_ValueChanged()
        {
            var connectionString = new SqlConnectionStringBuilder();

            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(connectionString);

            argumentsBuilder.Password = "The password";

            argumentsBuilder.Password.Should().Be("The password");
        }

        [Fact]
        public void Server_ValueChanged()
        {
            var connectionString = new SqlConnectionStringBuilder();

            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(connectionString);

            argumentsBuilder.Server = "The server";

            argumentsBuilder.Server.Should().Be("The server");
        }

        [Fact]
        public void LoginId_ValueChanged()
        {
            var connectionString = new SqlConnectionStringBuilder();

            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(connectionString);

            argumentsBuilder.LoginId = "The login";

            argumentsBuilder.LoginId.Should().Be("The login");
        }

        [Fact]
        public void TrustedConnection_ValueChanged()
        {
            var connectionString = new SqlConnectionStringBuilder();

            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(connectionString);

            argumentsBuilder.TrustedConnection = true;

            argumentsBuilder.TrustedConnection.Should().BeTrue();
        }

        [Fact]
        public void AddVariable()
        {
            var connectionString = new SqlConnectionStringBuilder();

            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(connectionString);

            argumentsBuilder.AddVariable("v1", "Value 1");
            argumentsBuilder.AddVariable("v2", "Value 2");

            argumentsBuilder.Variables.Should().HaveCount(2);

            argumentsBuilder.Variables["v1"].Should().Be("Value 1");
            argumentsBuilder.Variables["v2"].Should().Be("Value 2");
        }

        [Theory]
        [InlineData("TheServer", "TheDatabase", "TheLogin", "ThePassword", false, "TheInputFile", "-d \"TheDatabase\" -i \"TheInputFile\" -U \"TheLogin\" -P \"ThePassword\" -S \"TheServer\" -v v1=\"Value 1\" -b")]
        [InlineData("TheServer", "TheDatabase", null, null, true, "TheInputFile", "-d \"TheDatabase\" -i \"TheInputFile\" -E -S \"TheServer\" -v v1=\"Value 1\" -b")]
        [InlineData("TheServer", "TheDatabase", "", "", true, "TheInputFile", "-d \"TheDatabase\" -i \"TheInputFile\" -E -S \"TheServer\" -v v1=\"Value 1\" -b")]
        [InlineData("", "", "", "", false, "", "-v v1=\"Value 1\" -b")]
        [InlineData(null, null, null, null, false, null, "-v v1=\"Value 1\" -b")]
        public void ToString_ReturnsCommandLineArguments(string server, string database, string loginId, string password, bool trustedConnection, string inputFile, string expectedCommandLineArguments)
        {
            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(new SqlConnectionStringBuilder(string.Empty))
            {
                Database = database,
                InputFile = inputFile,
                LoginId = loginId,
                Password = password,
                Server = server,
                TrustedConnection = trustedConnection,
            };

            argumentsBuilder.AddVariable("v1", "Value 1");

            var commandLineArguments = argumentsBuilder.ToString();

            commandLineArguments.Should().Be(expectedCommandLineArguments);
        }

        [Theory]
        [InlineData("TheServer", "TheDatabase", "TheLogin", "ThePassword", false, "TheInputFile", "-d \"TheDatabase\" -i \"TheInputFile\" -U \"TheLogin\" -P \"ThePassword\" -S \"TheServer\" -b")]
        [InlineData("TheServer", "TheDatabase", null, null, true, "TheInputFile", "-d \"TheDatabase\" -i \"TheInputFile\" -E -S \"TheServer\" -b")]
        [InlineData("TheServer", "TheDatabase", "", "", true, "TheInputFile", "-d \"TheDatabase\" -i \"TheInputFile\" -E -S \"TheServer\" -b")]
        [InlineData("", "", "", "", false, "", "-b")]
        [InlineData(null, null, null, null, false, null, "-b")]
        public void ToString_ReturnsCommandLineArguments_WithNoVariables(string server, string database, string loginId, string password, bool trustedConnection, string inputFile, string expectedCommandLineArguments)
        {
            var argumentsBuilder = new SqlCmdCommandLineArgumentsBuilder(new SqlConnectionStringBuilder(string.Empty))
            {
                Database = database,
                InputFile = inputFile,
                LoginId = loginId,
                Password = password,
                Server = server,
                TrustedConnection = trustedConnection,
            };

            var commandLineArguments = argumentsBuilder.ToString();

            commandLineArguments.Should().Be(expectedCommandLineArguments);
        }
    }
}
