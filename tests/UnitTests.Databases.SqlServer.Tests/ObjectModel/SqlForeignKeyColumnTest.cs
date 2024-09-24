//-----------------------------------------------------------------------
// <copyright file="SqlForeignKeyColumnTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.Tests
{
    public class SqlForeignKeyColumnTest
    {
        [Fact]
        public void ToStringTest()
        {
            var column = new SqlForeignKeyColumn()
            {
                Name = "The name",
                Referenced = "The referenced",
                Position = default,
            };

            column.ToString().Should().Be("The name => The referenced");
        }
    }
}