//-----------------------------------------------------------------------
// <copyright file="SqlForeignKeyDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences of a <see cref="SqlForeignKey"/> between two databases.
    /// </summary>
    public class SqlForeignKeyDifferences : SqlObjectDifferences<SqlForeignKey>
    {
        internal SqlForeignKeyDifferences(
            SqlForeignKey? source,
            SqlForeignKey? target,
            SqlObjectDifferenceType type,
            IReadOnlyList<SqlObjectPropertyDifference>? properties,
            IList<SqlObjectDifferences<SqlForeignKeyColumn>> columns)
            : base(source, target, type, properties)
        {
            this.Columns = new ReadOnlyCollection<SqlObjectDifferences<SqlForeignKeyColumn>>(columns);
        }

        internal SqlForeignKeyDifferences(
            SqlObjectDifferences<SqlForeignKey> differences)
            : this(differences.Source, differences.Target, differences.Type, differences.Properties, [])
        {
        }

        /// <summary>
        /// Gets the difference of the columns in the foreign key compared.
        /// </summary>
        public ReadOnlyCollection<SqlObjectDifferences<SqlForeignKeyColumn>> Columns { get; }

        internal override void Accept(ISqlObjectDifferencesVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
