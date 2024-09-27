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
            var storedProcedure = new SqlStoredProcedure()
            {
                Name = "The name",
                Code = default,
                Schema = "The schema",
            };

            storedProcedure.ToString().Should().Be("The schema.The name");
        }
    }
}