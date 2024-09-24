//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseTableDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences of a <see cref="SqlTable"/> between two databases.
    /// </summary>
    public sealed class SqlDatabaseTableDifferences : SqlDatabaseObjectDifferences<SqlTable>
    {
        internal SqlDatabaseTableDifferences(
            SqlTable? source,
            SqlTable? target,
            SqlObjectDifferenceType type,
            IReadOnlyList<SqlObjectPropertyDifference>? properties,
            IList<SqlDatabaseObjectDifferences<SqlColumn>> columns,
            IList<SqlDatabaseObjectDifferences<SqlTrigger>> triggers,
            IList<SqlDatabaseObjectDifferences<SqlCheckConstraint>> checkConstraints,
            IList<SqlIndexDifferences> indexes,
            IList<SqlForeignKeyDifferences> foreignKeys,
            IList<SqlUniqueConstraintDifferences> uniqueConstraints)
            : base(source, target, type, properties)
        {
            this.Columns = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlColumn>>(columns);
            this.Triggers = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlTrigger>>(triggers);
            this.CheckConstraints = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlCheckConstraint>>(checkConstraints);
            this.Indexes = new ReadOnlyCollection<SqlIndexDifferences>(indexes);
            this.ForeignKeys = new ReadOnlyCollection<SqlForeignKeyDifferences>(foreignKeys);
            this.UniqueConstraints = new ReadOnlyCollection<SqlUniqueConstraintDifferences>(uniqueConstraints);
        }

        internal SqlDatabaseTableDifferences(
            SqlDatabaseObjectDifferences<SqlTable> differences)
            : base(differences.Source, differences.Target, differences.Type, differences.Properties)
        {
        }

        /// <summary>
        /// Gets the check constraint differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlCheckConstraint>> CheckConstraints { get; }

        /// <summary>
        /// Gets the columns differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlColumn>> Columns { get; }

        /// <summary>
        /// Gets the indexes differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlIndexDifferences> Indexes { get; }

        /// <summary>
        /// Gets the primary key differences between the two SQL tables.
        /// </summary>
        public required SqlPrimaryKeyDifferences? PrimaryKey { get; init; }

        /// <summary>
        /// Gets the foreign keys differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlForeignKeyDifferences> ForeignKeys { get; }

        /// <summary>
        /// Gets the triggers differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlTrigger>> Triggers { get; }

        /// <summary>
        /// Gets the unique constraints differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlUniqueConstraintDifferences> UniqueConstraints { get; }
    }
}
