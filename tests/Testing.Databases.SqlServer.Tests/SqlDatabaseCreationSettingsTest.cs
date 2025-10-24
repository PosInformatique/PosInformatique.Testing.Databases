//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseCreationSettingsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    public class SqlDatabaseCreationSettingsTest
    {
        [Fact]
        public void Constructor()
        {
            var settings = new SqlDatabaseCreationSettings();

            settings.DataFileName.Should().BeNull();
        }

        [Fact]
        public void DataFileName_ValueChanged()
        {
            var settings = new SqlDatabaseCreationSettings();

            settings.DataFileName = "New value";

            settings.DataFileName.Should().Be("New value");
        }
    }
}