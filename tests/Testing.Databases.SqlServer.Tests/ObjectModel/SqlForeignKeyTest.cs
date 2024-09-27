//-----------------------------------------------------------------------
// <copyright file="SqlForeignKeyTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.Tests
{
    public class SqlForeignKeyTest
    {
        [Fact]
        public void ToStringTest()
        {
            var foreignKey = new SqlForeignKey([])
            {
                DeleteAction = default,
                ReferencedTable = default,
                UpdateAction = default,
                Name = "The name",
            };

            foreignKey.ToString().Should().Be("The name");
        }
    }
}