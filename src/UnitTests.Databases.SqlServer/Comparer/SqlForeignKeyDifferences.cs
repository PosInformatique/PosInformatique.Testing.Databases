//-----------------------------------------------------------------------
// <copyright file="SqlForeignKeyDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences of a <see cref="SqlForeignKey"/> between two databases.
    /// </summary>
    public class SqlForeignKeyDifferences : SqlDatabaseObjectDifferences<SqlForeignKey>
    {
        internal SqlForeignKeyDifferences(
            SqlForeignKey? source,
            SqlForeignKey? target,
            SqlObjectDifferenceType type,
            IReadOnlyList<SqlObjectPropertyDifference>? properties,
            IList<SqlDatabaseObjectDifferences<SqlForeignKeyColumn>> columns)
            : base(source, target, type, properties)
        {
            this.Columns = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlForeignKeyColumn>>(columns);
        }

        internal SqlForeignKeyDifferences(
            SqlDatabaseObjectDifferences<SqlForeignKey> differences)
            : base(differences.Source, differences.Target, differences.Type, differences.Properties)
        {
        }

        /// <summary>
        /// Gets the difference of the columns in the foreign key compared.
        /// </summary>
        public ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlForeignKeyColumn>> Columns { get; }
    }
}
