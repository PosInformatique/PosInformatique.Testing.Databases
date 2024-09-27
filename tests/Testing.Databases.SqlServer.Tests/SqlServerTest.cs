//-----------------------------------------------------------------------
// <copyright file="SqlServerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    public class SqlServerTest
    {
        [Theory]
        [InlineData("Data Source=TheServer; Initial Catalog=TheDB; User ID=TheID; Password=ThePassword", "Data Source=TheServer;Initial Catalog=master;User ID=TheID;Password=ThePassword")]
        [InlineData("Data Source=TheServer; Initial Catalog=TheDB; Integrated Security=True", "Data Source=TheServer;Initial Catalog=master;Integrated Security=True")]
        public void Constructor(string connectionString, string expectedMasterConnectionString)
        {
            var server = new SqlServer(connectionString);

            server.Master.ConnectionString.Should().Be(expectedMasterConnectionString);
            server.Master.Server.Should().BeSameAs(server);
        }
    }
}