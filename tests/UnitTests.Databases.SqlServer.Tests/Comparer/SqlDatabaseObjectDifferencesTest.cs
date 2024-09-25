//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseObjectDifferencesTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.Tests
{
    public class SqlDatabaseObjectDifferencesTest
    {
        [Fact]
        public void ToStringTest()
        {
            var source = new SqlUserType()
            {
                IsNullable = default,
                IsTableType = default,
                MaxLength = default,
                Name = "The source",
                SystemTypeId = default,
            };

            var target = new SqlUserType()
            {
                IsNullable = default,
                IsTableType = default,
                MaxLength = default,
                Name = "The source",
                SystemTypeId = default,
            };

            var properties = new[]
            {
                new SqlObjectPropertyDifference("The prop1", 10, 20),
                new SqlObjectPropertyDifference("The prop2", 30, 40),
            };

            var difference = new SqlDatabaseObjectDifferences<SqlUserType>(source, target, default, properties);

            difference.ToString().Should().Be("* The prop1:\r\n      Source: 10\r\n      Target: 20\r\n  * The prop2:\r\n      Source: 30\r\n      Target: 40\r\n");
        }
    }
}