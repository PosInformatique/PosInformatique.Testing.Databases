//-----------------------------------------------------------------------
// <copyright file="SqlCheckConstraintTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.Tests
{
    public class SqlCheckConstraintTest
    {
        [Fact]
        public void ToStringTest()
        {
            var checkConstraint = new SqlCheckConstraint()
            {
                Name = "The name",
                Code = default,
            };

            checkConstraint.ToString().Should().Be("The name");
        }
    }
}