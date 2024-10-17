//-----------------------------------------------------------------------
// <copyright file="SqlColumnTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlColumnTest
    {
        [Fact]
        public void ToStringTest()
        {
            var column = new SqlColumn("The name", default, default, default, default, default);

            column.ToString().Should().Be("The name");
        }
    }
}