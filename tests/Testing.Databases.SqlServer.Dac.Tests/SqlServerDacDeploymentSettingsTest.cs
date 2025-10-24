//-----------------------------------------------------------------------
// <copyright file="SqlServerDacDeploymentSettingsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    public class SqlServerDacDeploymentSettingsTest
    {
        [Fact]
        public void Constructor()
        {
            var settings = new SqlServerDacDeploymentSettings();

            settings.DataFileName.Should().BeNull();
        }

        [Fact]
        public void DataFileName_ValueChanged()
        {
            var settings = new SqlServerDacDeploymentSettings();

            settings.DataFileName = "The value";

            settings.DataFileName.Should().Be("The value");
        }
    }
}