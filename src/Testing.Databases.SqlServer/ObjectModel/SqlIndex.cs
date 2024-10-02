//-----------------------------------------------------------------------
// <copyright file="SqlIndex.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents an index of a SQL table.
    /// </summary>
    public sealed class SqlIndex : SqlObject
    {
        internal SqlIndex(string name, string type, IList<SqlIndexColumn> columns, IList<SqlIndexColumn> includedColumns)
        {
            this.Name = name;
            this.Type = type;
            this.Columns = new ReadOnlyCollection<SqlIndexColumn>(columns);
            this.IncludedColumns = new ReadOnlyCollection<SqlIndexColumn>(includedColumns);
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the columns of the index.
        /// </summary>
        public ReadOnlyCollection<SqlIndexColumn> Columns { get; }

        /// <summary>
        /// Gets the included columns of the index.
        /// </summary>
        public ReadOnlyCollection<SqlIndexColumn> IncludedColumns { get; }

        /// <summary>
        /// Gets the filter of the index.
        /// </summary>
        public string? Filter { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether if the index has unique values.
        /// </summary>
        public bool IsUnique { get; internal set; }

        /// <summary>
        /// Gets the type of the index.
        /// </summary>
        public string Type { get; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
