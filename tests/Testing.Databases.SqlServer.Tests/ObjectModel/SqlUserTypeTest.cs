//-----------------------------------------------------------------------
// <copyright file="SqlUserTypeTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlUserTypeTest
    {
        [Fact]
        public void ToStringTest()
        {
            var userType = new SqlUserType()
            {
                Name = "The name",
                IsNullable = default,
                IsTableType = default,
                MaxLength = default,
            };

            userType.ToString().Should().Be("The name");
        }
    }
}