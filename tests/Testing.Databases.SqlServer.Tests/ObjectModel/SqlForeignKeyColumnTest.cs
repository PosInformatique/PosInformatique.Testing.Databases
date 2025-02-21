﻿//-----------------------------------------------------------------------
// <copyright file="SqlForeignKeyColumnTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlForeignKeyColumnTest
    {
        [Fact]
        public void ToStringTest()
        {
            var column = new SqlForeignKeyColumn("The name", default, "The referenced");

            column.ToString().Should().Be("The name => The referenced");
        }
    }
}