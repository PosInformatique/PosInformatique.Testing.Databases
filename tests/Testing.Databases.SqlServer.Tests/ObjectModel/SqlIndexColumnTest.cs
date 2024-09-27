//-----------------------------------------------------------------------
// <copyright file="SqlIndexColumnTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlIndexColumnTest
    {
        [Fact]
        public void ToStringTest()
        {
            var column = new SqlIndexColumn()
            {
                Name = "The name",
                Position = default,
            };

            column.ToString().Should().Be("The name");
        }
    }
}