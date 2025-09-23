//-----------------------------------------------------------------------
// <copyright file="SqlCmdProcessTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    using Microsoft.Data.SqlClient;

    public class SqlCmdProcessTest
    {
        [Fact]
        public void WaitForExit_Disposed()
        {
            var process = SqlCmdProcess.RunScript(new SqlConnectionStringBuilder(), "NoFile.sql", new SqlCmdRunScriptSettings());

            process.Dispose();

            process.Invoking(p => p.WaitForExit())
                .Should().ThrowExactly<ObjectDisposedException>()
                .WithMessage("Cannot access a disposed object.\r\nObject name: 'PosInformatique.Testing.Databases.SqlServer.SqlCmdProcess'.")
                .Which.ObjectName.Should().Be("PosInformatique.Testing.Databases.SqlServer.SqlCmdProcess");
        }
    }
}