//-----------------------------------------------------------------------
// <copyright file="SqlColumnIdentityTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlColumnIdentityTest
    {
        [Fact]
        public void ToStringTest()
        {
            var column = new SqlColumnIdentity(1, 2);

            column.ToString().Should().Be("(Seed: 1, Increment: 2)");
        }
    }
}