//-----------------------------------------------------------------------
// <copyright file="SqlIndexDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences of a <see cref="SqlIndex"/> between two databases.
    /// </summary>
    public class SqlIndexDifferences : SqlDatabaseObjectDifferences<SqlIndex>
    {
        internal SqlIndexDifferences(
            SqlIndex? source,
            SqlIndex? target,
            SqlObjectDifferenceType type,
            IReadOnlyList<SqlObjectPropertyDifference>? properties,
            IList<SqlDatabaseObjectDifferences<SqlIndexColumn>> columns,
            IList<SqlDatabaseObjectDifferences<SqlIndexColumn>> includedColumns)
            : base(source, target, type, properties)
        {
            this.Columns = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlIndexColumn>>(columns);
            this.IncludedColumns = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlIndexColumn>>(includedColumns);
        }

        internal SqlIndexDifferences(
            SqlDatabaseObjectDifferences<SqlIndex> differences)
            : base(differences.Source, differences.Target, differences.Type, differences.Properties)
        {
        }

        /// <summary>
        /// Gets the columns differences in the index compared.
        /// </summary>
        public ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlIndexColumn>> Columns { get; }

        /// <summary>
        /// Gets the included columns differences in the index compared.
        /// </summary>
        public ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlIndexColumn>> IncludedColumns { get; }
    }
}
