//-----------------------------------------------------------------------
// <copyright file="SqlColumnDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents the differences of a <see cref="SqlColumn"/> between two databases.
    /// </summary>
    public class SqlColumnDifferences : SqlObjectDifferences<SqlColumn>
    {
        internal SqlColumnDifferences(
            SqlColumn? source,
            SqlColumn? target,
            SqlObjectDifferenceType type,
            IReadOnlyList<SqlObjectPropertyDifference>? properties,
            SqlObjectDifferences<SqlDefaultConstraint>? defaultConstraint)
            : base(source, target, type, properties)
        {
            this.DefaultConstraint = defaultConstraint;
        }

        internal SqlColumnDifferences(
            SqlObjectDifferences<SqlColumn> differences)
            : this(differences.Source, differences.Target, differences.Type, differences.Properties, null)
        {
        }

        /// <summary>
        /// Gets the difference of the columns in the foreign key compared.
        /// </summary>
        public SqlObjectDifferences<SqlDefaultConstraint>? DefaultConstraint { get; }

        internal override void Accept(ISqlObjectDifferencesVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
