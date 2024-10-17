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
            var userType = new SqlUserType("The name", default);

            userType.ToString().Should().Be("The name");
        }
    }
}