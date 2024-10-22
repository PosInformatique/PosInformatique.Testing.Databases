﻿//-----------------------------------------------------------------------
// <copyright file="SqlIndexTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlIndexTest
    {
        [Fact]
        public void ToStringTest()
        {
            var index = new SqlIndex("The name", default, Array.Empty<SqlIndexColumn>(), Array.Empty<SqlIndexColumn>());

            index.ToString().Should().Be("The name");
        }
    }
}