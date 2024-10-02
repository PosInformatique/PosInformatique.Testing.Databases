//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseComparisonResultsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.Tests
{
    using System.Collections.ObjectModel;

    public class SqlDatabaseComparisonResultsTest
    {
        [Fact]
        public void IsIdentical()
        {
            var results = new SqlDatabaseComparisonResults([], [], [], []);

            results.IsIdentical.Should().BeTrue();
        }

        [Fact]
        public void IsIdentical_StoredProcedure()
        {
            var results = new SqlDatabaseComparisonResults([null], [], [], []);

            results.IsIdentical.Should().BeFalse();
        }

        [Fact]
        public void IsIdentical_Tables()
        {
            var results = new SqlDatabaseComparisonResults([], [null], [], []);

            results.IsIdentical.Should().BeFalse();
        }

        [Fact]
        public void IsIdentical_UserTypes()
        {
            var results = new SqlDatabaseComparisonResults([], [], [null], []);

            results.IsIdentical.Should().BeFalse();
        }

        [Fact]
        public void IsIdentical_Views()
        {
            var results = new SqlDatabaseComparisonResults([], [], [], [null]);

            results.IsIdentical.Should().BeFalse();
        }
    }
}