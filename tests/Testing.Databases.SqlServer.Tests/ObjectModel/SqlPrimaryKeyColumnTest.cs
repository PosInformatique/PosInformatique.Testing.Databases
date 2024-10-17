//-----------------------------------------------------------------------
// <copyright file="SqlPrimaryKeyColumnTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlPrimaryKeyColumnTest
    {
        [Fact]
        public void ToStringTest()
        {
            var column = new SqlPrimaryKeyColumn("The name", default);

            column.ToString().Should().Be("The name");
        }
    }
}