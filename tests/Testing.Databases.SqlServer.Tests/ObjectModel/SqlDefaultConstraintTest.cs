//-----------------------------------------------------------------------
// <copyright file="SqlDefaultConstraintTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlDefaultConstraintTest
    {
        [Fact]
        public void ToStringTest()
        {
            var defaultConstraint = new SqlDefaultConstraint("The name", default);

            defaultConstraint.ToString().Should().Be("The name");
        }
    }
}