//-----------------------------------------------------------------------
// <copyright file="SqlStoredProcedureTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlStoredProcedureTest
    {
        [Fact]
        public void ToStringTest()
        {
            var storedProcedure = new SqlStoredProcedure("The schema", "The name", default);

            storedProcedure.ToString().Should().Be("The schema.The name");
        }
    }
}