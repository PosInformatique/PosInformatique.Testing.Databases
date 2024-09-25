//-----------------------------------------------------------------------
// <copyright file="SqlIndexDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences of a <see cref="SqlIndex"/> between two databases.
    /// </summary>
    public class SqlIndexDifferences : SqlObjectDifferences<SqlIndex>
    {
        internal SqlIndexDifferences(
            SqlIndex? source,
            SqlIndex? target,
            SqlObjectDifferenceType type,
            IReadOnlyList<SqlObjectPropertyDifference>? properties,
            IList<SqlObjectDifferences<SqlIndexColumn>> columns,
            IList<SqlObjectDifferences<SqlIndexColumn>> includedColumns)
            : base(source, target, type, properties)
        {
            this.Columns = new ReadOnlyCollection<SqlObjectDifferences<SqlIndexColumn>>(columns);
            this.IncludedColumns = new ReadOnlyCollection<SqlObjectDifferences<SqlIndexColumn>>(includedColumns);
        }

        internal SqlIndexDifferences(
            SqlObjectDifferences<SqlIndex> differences)
            : this(differences.Source, differences.Target, differences.Type, differences.Properties, [], [])
        {
        }

        /// <summary>
        /// Gets the columns differences in the index compared.
        /// </summary>
        public ReadOnlyCollection<SqlObjectDifferences<SqlIndexColumn>> Columns { get; }

        /// <summary>
        /// Gets the included columns differences in the index compared.
        /// </summary>
        public ReadOnlyCollection<SqlObjectDifferences<SqlIndexColumn>> IncludedColumns { get; }

        internal override void Accept(ISqlObjectDifferencesVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
