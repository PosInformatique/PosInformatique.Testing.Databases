//-----------------------------------------------------------------------
// <copyright file="SqlObjectDifferencesTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlObjectDifferencesTest
    {
        [Fact]
        public void ToStringTest()
        {
            var source = new SqlUserType("The source", default);
            var target = new SqlUserType("The source", default);

            var properties = new[]
            {
                new SqlObjectPropertyDifference("The prop1", 10, 20),
                new SqlObjectPropertyDifference("The prop2", 30, 40),
            };

            var difference = new SqlObjectDifferences<SqlUserType>(source, target, default, properties);

            difference.ToString().Should().Be($"The source{Environment.NewLine}  * The prop1:{Environment.NewLine}      Source: 10{Environment.NewLine}      Target: 20{Environment.NewLine}  * The prop2:{Environment.NewLine}      Source: 30{Environment.NewLine}      Target: 40{Environment.NewLine}");
        }
    }
}