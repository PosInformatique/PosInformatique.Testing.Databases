//-----------------------------------------------------------------------
// <copyright file="SqlPrimaryKeyTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlPrimaryKeyTest
    {
        [Fact]
        public void ToStringTest()
        {
            var primaryKey = new SqlPrimaryKey("The name", default, []);

            primaryKey.ToString().Should().Be("The name");
        }
    }
}