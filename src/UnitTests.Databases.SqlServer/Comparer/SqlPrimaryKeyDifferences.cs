//-----------------------------------------------------------------------
// <copyright file="SqlPrimaryKeyDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences of a <see cref="SqlPrimaryKey"/> between two databases.
    /// </summary>
    public class SqlPrimaryKeyDifferences : SqlDatabaseObjectDifferences<SqlPrimaryKey>
    {
        internal SqlPrimaryKeyDifferences(
            SqlPrimaryKey? source,
            SqlPrimaryKey? target,
            SqlObjectDifferenceType type,
            IReadOnlyList<SqlObjectPropertyDifference>? properties,
            IList<SqlDatabaseObjectDifferences<SqlPrimaryKeyColumn>> columns)
            : base(source, target, type, properties)
        {
            this.Columns = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlPrimaryKeyColumn>>(columns);
        }

        /// <summary>
        /// Gets the difference of the columns in the primary key compared.
        /// </summary>
        public ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlPrimaryKeyColumn>> Columns { get; }
    }
}
