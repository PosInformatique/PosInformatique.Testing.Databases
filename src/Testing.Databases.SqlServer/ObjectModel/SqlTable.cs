//-----------------------------------------------------------------------
// <copyright file="SqlTable.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents a table in SQL database.
    /// </summary>
    public sealed class SqlTable : SqlObject
    {
        internal SqlTable(
            string schema,
            string name,
            IList<SqlColumn> columns,
            IList<SqlTrigger> triggers,
            IList<SqlCheckConstraint> checkConstraints,
            IList<SqlIndex> indexes,
            IList<SqlForeignKey> foreignKeys,
            IList<SqlUniqueConstraint> uniqueConstraints)
        {
            this.Schema = schema;
            this.Name = name;

            this.CheckConstraints = new ReadOnlyCollection<SqlCheckConstraint>(checkConstraints);
            this.Columns = new ReadOnlyCollection<SqlColumn>(columns);
            this.Indexes = new ReadOnlyCollection<SqlIndex>(indexes);
            this.Triggers = new ReadOnlyCollection<SqlTrigger>(triggers);
            this.ForeignKeys = new ReadOnlyCollection<SqlForeignKey>(foreignKeys);
            this.UniqueConstraints = new ReadOnlyCollection<SqlUniqueConstraint>(uniqueConstraints);
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the check constraints of the table.
        /// </summary>
        public ReadOnlyCollection<SqlCheckConstraint> CheckConstraints { get; }

        /// <summary>
        /// Gets the columns of the table.
        /// </summary>
        public ReadOnlyCollection<SqlColumn> Columns { get; }

        /// <summary>
        /// Gets the foreign keys of the table.
        /// </summary>
        public ReadOnlyCollection<SqlForeignKey> ForeignKeys { get; }

        /// <summary>
        /// Gets the indexes of the table.
        /// </summary>
        public ReadOnlyCollection<SqlIndex> Indexes { get; }

        /// <summary>
        /// Gets the primary key of the table.
        /// </summary>
        public SqlPrimaryKey? PrimaryKey { get; internal set; }

        /// <summary>
        /// Gets the schema which the table belong to.
        /// </summary>
        public string Schema { get; }

        /// <summary>
        /// Gets the triggers of the table.
        /// </summary>
        public ReadOnlyCollection<SqlTrigger> Triggers { get; }

        /// <summary>
        /// Gets the unique constraint of the table.
        /// </summary>
        public ReadOnlyCollection<SqlUniqueConstraint> UniqueConstraints { get; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return $"{this.Schema}.{this.Name}";
        }
    }
}
