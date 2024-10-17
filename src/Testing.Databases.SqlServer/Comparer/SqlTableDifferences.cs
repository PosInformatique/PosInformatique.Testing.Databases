//-----------------------------------------------------------------------
// <copyright file="SqlTableDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences of a <see cref="SqlTable"/> between two databases.
    /// </summary>
    public sealed class SqlTableDifferences : SqlObjectDifferences<SqlTable>
    {
        internal SqlTableDifferences(
            SqlTable? source,
            SqlTable? target,
            SqlObjectDifferenceType type,
            IReadOnlyList<SqlObjectPropertyDifference>? properties,
            IList<SqlObjectDifferences<SqlColumn>> columns,
            IList<SqlObjectDifferences<SqlTrigger>> triggers,
            IList<SqlObjectDifferences<SqlCheckConstraint>> checkConstraints,
            IList<SqlIndexDifferences> indexes,
            IList<SqlForeignKeyDifferences> foreignKeys,
            IList<SqlUniqueConstraintDifferences> uniqueConstraints)
            : base(source, target, type, properties)
        {
            this.Columns = new ReadOnlyCollection<SqlObjectDifferences<SqlColumn>>(columns);
            this.Triggers = new ReadOnlyCollection<SqlObjectDifferences<SqlTrigger>>(triggers);
            this.CheckConstraints = new ReadOnlyCollection<SqlObjectDifferences<SqlCheckConstraint>>(checkConstraints);
            this.Indexes = new ReadOnlyCollection<SqlIndexDifferences>(indexes);
            this.ForeignKeys = new ReadOnlyCollection<SqlForeignKeyDifferences>(foreignKeys);
            this.UniqueConstraints = new ReadOnlyCollection<SqlUniqueConstraintDifferences>(uniqueConstraints);
        }

        internal SqlTableDifferences(
            SqlObjectDifferences<SqlTable> differences)
            : this(differences.Source, differences.Target, differences.Type, differences.Properties, [], [], [], [], [], [])
        {
        }

        /// <summary>
        /// Gets the check constraint differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlObjectDifferences<SqlCheckConstraint>> CheckConstraints { get; }

        /// <summary>
        /// Gets the columns differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlObjectDifferences<SqlColumn>> Columns { get; }

        /// <summary>
        /// Gets the indexes differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlIndexDifferences> Indexes { get; }

        /// <summary>
        /// Gets the primary key differences between the two SQL tables.
        /// </summary>
        public SqlPrimaryKeyDifferences? PrimaryKey { get; internal set; }

        /// <summary>
        /// Gets the foreign keys differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlForeignKeyDifferences> ForeignKeys { get; }

        /// <summary>
        /// Gets the triggers differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlObjectDifferences<SqlTrigger>> Triggers { get; }

        /// <summary>
        /// Gets the unique constraints differences between the two SQL tables.
        /// </summary>
        public ReadOnlyCollection<SqlUniqueConstraintDifferences> UniqueConstraints { get; }

        internal override void Accept(ISqlObjectDifferencesVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
