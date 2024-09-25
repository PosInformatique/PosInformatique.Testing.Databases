//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseComparisonResultsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.Tests
{
    using System.Collections.ObjectModel;

    public class SqlDatabaseComparisonResultsTest
    {
        [Fact]
        public void IsIdentical()
        {
            var results = new SqlDatabaseComparisonResults()
            {
                StoredProcedures = new ReadOnlyCollection<SqlObjectDifferences<SqlStoredProcedure>>([null]),
                Tables = new ReadOnlyCollection<SqlTableDifferences>([null]),
                UserTypes = new ReadOnlyCollection<SqlObjectDifferences<SqlUserType>>([null]),
                Views = new ReadOnlyCollection<SqlObjectDifferences<SqlView>>([null]),
            };

            results.IsIdentical.Should().BeFalse();
        }

        [Fact]
        public void IsIdentical_StoredProcedure()
        {
            var results = new SqlDatabaseComparisonResults()
            {
                StoredProcedures = new ReadOnlyCollection<SqlObjectDifferences<SqlStoredProcedure>>([null]),
                Tables = new ReadOnlyCollection<SqlTableDifferences>([]),
                UserTypes = new ReadOnlyCollection<SqlObjectDifferences<SqlUserType>>([]),
                Views = new ReadOnlyCollection<SqlObjectDifferences<SqlView>>([]),
            };

            results.IsIdentical.Should().BeFalse();
        }

        [Fact]
        public void IsIdentical_Tables()
        {
            var results = new SqlDatabaseComparisonResults()
            {
                StoredProcedures = new ReadOnlyCollection<SqlObjectDifferences<SqlStoredProcedure>>([]),
                Tables = new ReadOnlyCollection<SqlTableDifferences>([null]),
                UserTypes = new ReadOnlyCollection<SqlObjectDifferences<SqlUserType>>([]),
                Views = new ReadOnlyCollection<SqlObjectDifferences<SqlView>>([]),
            };

            results.IsIdentical.Should().BeFalse();
        }

        [Fact]
        public void IsIdentical_UserTypes()
        {
            var results = new SqlDatabaseComparisonResults()
            {
                StoredProcedures = new ReadOnlyCollection<SqlObjectDifferences<SqlStoredProcedure>>([]),
                Tables = new ReadOnlyCollection<SqlTableDifferences>([]),
                UserTypes = new ReadOnlyCollection<SqlObjectDifferences<SqlUserType>>([null]),
                Views = new ReadOnlyCollection<SqlObjectDifferences<SqlView>>([]),
            };

            results.IsIdentical.Should().BeFalse();
        }

        [Fact]
        public void IsIdentical_Views()
        {
            var results = new SqlDatabaseComparisonResults()
            {
                StoredProcedures = new ReadOnlyCollection<SqlObjectDifferences<SqlStoredProcedure>>([]),
                Tables = new ReadOnlyCollection<SqlTableDifferences>([]),
                UserTypes = new ReadOnlyCollection<SqlObjectDifferences<SqlUserType>>([]),
                Views = new ReadOnlyCollection<SqlObjectDifferences<SqlView>>([null]),
            };

            results.IsIdentical.Should().BeFalse();
        }
    }
}