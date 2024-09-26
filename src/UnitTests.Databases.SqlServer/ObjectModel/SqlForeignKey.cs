//-----------------------------------------------------------------------
// <copyright file="SqlForeignKey.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents an index of a SQL table.
    /// </summary>
    public sealed class SqlForeignKey : SqlObject
    {
        internal SqlForeignKey(IList<SqlForeignKeyColumn> columns)
        {
            this.Columns = new ReadOnlyCollection<SqlForeignKeyColumn>(columns);
        }

        /// <summary>
        /// Gets the name of the foreign key.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the columns of the foreign key.
        /// </summary>
        public ReadOnlyCollection<SqlForeignKeyColumn> Columns { get; }

        /// <summary>
        /// Gets the name of the referenced table.
        /// </summary>
        public required string ReferencedTable { get; init; }

        /// <summary>
        /// Gets the referential update action.
        /// </summary>
        public required string UpdateAction { get; init; }

        /// <summary>
        /// Gets the referential delete action.
        /// </summary>
        public required string DeleteAction { get; init; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
