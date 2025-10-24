//-----------------------------------------------------------------------
// <copyright file="SqlCmdRunScriptSettingsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    public class SqlCmdRunScriptSettingsTest
    {
        [Fact]
        public void Constructor()
        {
            var settings = new SqlCmdRunScriptSettings();

            settings.Variables.Should().BeEmpty();
        }
    }
}