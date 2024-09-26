//-----------------------------------------------------------------------
// <copyright file="SqlTableTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.Tests
{
    public class SqlTableTest
    {
        [Fact]
        public void ToStringTest()
        {
            var table = new SqlTable([], [], [], [], [], [])
            {
                Name = "The name",
                PrimaryKey = default,
                Schema = "The schema",
            };

            table.ToString().Should().Be("The schema.The name");
        }
    }
}