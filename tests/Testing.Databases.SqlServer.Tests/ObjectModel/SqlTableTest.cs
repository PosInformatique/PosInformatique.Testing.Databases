//-----------------------------------------------------------------------
// <copyright file="SqlTableTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlTableTest
    {
        [Fact]
        public void ToStringTest()
        {
            var table = new SqlTable("The schema", "The name", [], [], [], [], [], []);

            table.ToString().Should().Be("The schema.The name");
        }
    }
}