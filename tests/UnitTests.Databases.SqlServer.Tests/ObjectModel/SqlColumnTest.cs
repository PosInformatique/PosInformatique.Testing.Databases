//-----------------------------------------------------------------------
// <copyright file="SqlColumnTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.Tests
{
    public class SqlColumnTest
    {
        [Fact]
        public void ToStringTest()
        {
            var column = new SqlColumn()
            {
                Name = "The name",
                CollationName = default,
                ComputedExpression = default,
                IsComputed = default,
                IsIdentity = default,
                IsNullable = default,
                MaxLength = default,
                Position = default,
                Precision = default,
                Scale = default,
                SystemTypeId = default, 
                TypeName = default,
            };

            column.ToString().Should().Be("The name");
        }
    }
}