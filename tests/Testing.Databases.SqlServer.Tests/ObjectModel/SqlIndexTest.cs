//-----------------------------------------------------------------------
// <copyright file="SqlIndexTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.Tests
{
    public class SqlIndexTest
    {
        [Fact]
        public void ToStringTest()
        {
            var index = new SqlIndex(Array.Empty<SqlIndexColumn>(), Array.Empty<SqlIndexColumn>())
            {
                Filter = null,
                IsUnique = default,
                Name = "The name",
                Type = default,
            };

            index.ToString().Should().Be("The name");
        }
    }
}