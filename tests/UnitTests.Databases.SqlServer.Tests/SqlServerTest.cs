﻿//-----------------------------------------------------------------------
// <copyright file="SqlServerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer.Tests
{
    public class SqlServerTest
    {
        [Theory]
        [InlineData("Data Source=TheServer; Initial Catalog=TheDB; User ID=TheID; Password=ThePassword", "Data Source=TheServer;Initial Catalog=master;Integrated Security=True")]
        [InlineData("Data Source=TheServer; Initial Catalog=TheDB; Integrated Security=True", "Data Source=TheServer;Initial Catalog=master;Integrated Security=True")]
        public void Constructor(string connectionString, string expectedMasterConnectionString)
        {
            var server = new SqlServer(connectionString);

            server.Master.ConnectionString.Should().Be(expectedMasterConnectionString);
        }
    }
}