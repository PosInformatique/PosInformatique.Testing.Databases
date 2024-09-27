//-----------------------------------------------------------------------
// <copyright file="SqlTriggerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    public class SqlTriggerTest
    {
        [Fact]
        public void ToStringTest()
        {
            var trigger = new SqlTrigger()
            {
                Name = "The name",
                Code = default,
            };

            trigger.ToString().Should().Be("The name");
        }
    }
}