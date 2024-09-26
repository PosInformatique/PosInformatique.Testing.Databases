//-----------------------------------------------------------------------
// <copyright file="SqlViewTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.Tests
{
    public class SqlViewTest
    {
        [Fact]
        public void ToStringTest()
        {
            var view = new SqlView()
            {
                Name = "The name",
                Code = default,
                Schema = "The schema",
            };

            view.ToString().Should().Be("The schema.The name");
        }
    }
}