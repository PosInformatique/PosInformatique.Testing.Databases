//-----------------------------------------------------------------------
// <copyright file="SqlObjectDifferencesTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlObjectDifferencesTest
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
            };

            var target = new SqlUserType()
            {
                IsNullable = default,
                IsTableType = default,
                MaxLength = default,
                Name = "The source",
            };

            var properties = new[]
            {
                new SqlObjectPropertyDifference("The prop1", 10, 20),
                new SqlObjectPropertyDifference("The prop2", 30, 40),
            };

            var difference = new SqlObjectDifferences<SqlUserType>(source, target, default, properties);

            difference.ToString().Should().Be("The source\r\n  * The prop1:\r\n      Source: 10\r\n      Target: 20\r\n  * The prop2:\r\n      Source: 30\r\n      Target: 40\r\n");
        }
    }
}