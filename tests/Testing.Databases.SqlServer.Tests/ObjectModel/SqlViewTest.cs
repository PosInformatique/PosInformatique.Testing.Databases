//-----------------------------------------------------------------------
// <copyright file="SqlViewTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlViewTest
    {
        [Fact]
        public void ToStringTest()
        {
            var view = new SqlView("The schema", "The name", default);

            view.ToString().Should().Be("The schema.The name");
        }
    }
}