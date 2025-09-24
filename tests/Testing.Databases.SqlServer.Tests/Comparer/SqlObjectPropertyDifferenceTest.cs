//-----------------------------------------------------------------------
// <copyright file="SqlObjectPropertyDifferenceTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlObjectPropertyDifferenceTest
    {
        [Fact]
        public void ToString_NotNull()
        {
            var difference = new SqlObjectPropertyDifference("The name", 12, 34);

            difference.ToString().Should().Be($"* The name:{Environment.NewLine}    Source: 12{Environment.NewLine}    Target: 34{Environment.NewLine}");
        }

        [Fact]
        public void ToString_Null()
        {
            var difference = new SqlObjectPropertyDifference("The name", null, null);

            difference.ToString().Should().Be($"* The name:{Environment.NewLine}    Source: <No value>{Environment.NewLine}    Target: <No value>{Environment.NewLine}");
        }
    }
}