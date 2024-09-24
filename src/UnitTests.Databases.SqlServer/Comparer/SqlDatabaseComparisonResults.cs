//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseComparisonResults.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences between 2 databases.
    /// </summary>
    public class SqlDatabaseComparisonResults
    {
        internal SqlDatabaseComparisonResults()
        {
        }

        /// <summary>
        /// Gets the stored procedures which are different between two databases.
        /// </summary>
        public required ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlStoredProcedure>> StoredProcedures { get; init; }

        /// <summary>
        /// Gets the tables which are different between two databases.
        /// </summary>
        public required ReadOnlyCollection<SqlDatabaseTableDifferences> Tables { get; init; }

        /// <summary>
        /// Gets the user types which are different between two databases.
        /// </summary>
        public required ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlUserType>> UserTypes { get; init; }

        /// <summary>
        /// Gets the views which are different between two databases.
        /// </summary>
        public required ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlView>> Views { get; init; }
    }
}
