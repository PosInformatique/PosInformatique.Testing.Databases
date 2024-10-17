//-----------------------------------------------------------------------
// <copyright file="SqlForeignKeyTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlForeignKeyTest
    {
        [Fact]
        public void ToStringTest()
        {
            var foreignKey = new SqlForeignKey("The name", default, default, default, []);

            foreignKey.ToString().Should().Be("The name");
        }
    }
}