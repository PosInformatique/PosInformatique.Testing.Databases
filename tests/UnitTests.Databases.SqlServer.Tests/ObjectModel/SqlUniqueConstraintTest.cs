//-----------------------------------------------------------------------
// <copyright file="SqlUniqueConstraintTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.Tests
{
    public class SqlUniqueConstraintTest
    {
        [Fact]
        public void ToStringTest()
        {
            var uniqueConstraint = new SqlUniqueConstraint([])
            {
                Name = "The name",
                Type = default,
            };

            uniqueConstraint.ToString().Should().Be("The name");
        }
    }
}