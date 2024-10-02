//-----------------------------------------------------------------------
// <copyright file="SqlForeignKey.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents an index of a SQL table.
    /// </summary>
    public sealed class SqlForeignKey : SqlObject
    {
        internal SqlForeignKey(
            string name,
            string referencedTable,
            string updateAction,
            string deleteAction,
            IList<SqlForeignKeyColumn> columns)
        {
            this.Name = name;
            this.ReferencedTable = referencedTable;
            this.UpdateAction = updateAction;
            this.DeleteAction = deleteAction;

            this.Columns = new ReadOnlyCollection<SqlForeignKeyColumn>(columns);
        }

        /// <summary>
        /// Gets the name of the foreign key.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the columns of the foreign key.
        /// </summary>
        public ReadOnlyCollection<SqlForeignKeyColumn> Columns { get; }

        /// <summary>
        /// Gets the name of the referenced table.
        /// </summary>
        public string ReferencedTable { get; }

        /// <summary>
        /// Gets the referential update action.
        /// </summary>
        public string UpdateAction { get; }

        /// <summary>
        /// Gets the referential delete action.
        /// </summary>
        public string DeleteAction { get; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
