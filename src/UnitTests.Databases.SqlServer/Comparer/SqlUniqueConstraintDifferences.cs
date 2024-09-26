//-----------------------------------------------------------------------
// <copyright file="SqlUniqueConstraintDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences of a <see cref="SqlUniqueConstraintDifferences"/> between two databases.
    /// </summary>
    public class SqlUniqueConstraintDifferences : SqlObjectDifferences<SqlUniqueConstraint>
    {
        internal SqlUniqueConstraintDifferences(
            SqlUniqueConstraint? source,
            SqlUniqueConstraint? target,
            SqlObjectDifferenceType type,
            IReadOnlyList<SqlObjectPropertyDifference>? properties,
            IList<SqlObjectDifferences<SqlIndexColumn>> columns)
            : base(source, target, type, properties)
        {
            this.Columns = new ReadOnlyCollection<SqlObjectDifferences<SqlIndexColumn>>(columns);
        }

        internal SqlUniqueConstraintDifferences(
            SqlObjectDifferences<SqlUniqueConstraint> differences)
            : this(differences.Source, differences.Target, differences.Type, differences.Properties, [])
        {
        }

        /// <summary>
        /// Gets the difference of the columns in the unique constraint compared.
        /// </summary>
        public ReadOnlyCollection<SqlObjectDifferences<SqlIndexColumn>> Columns { get; }

        internal override void Accept(ISqlObjectDifferencesVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
