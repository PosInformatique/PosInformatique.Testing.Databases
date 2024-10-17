//-----------------------------------------------------------------------
// <copyright file="SqlPrimaryKey.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents a primary key of a SQL table.
    /// </summary>
    public sealed class SqlPrimaryKey : SqlObject
    {
        internal SqlPrimaryKey(string name, string type, IList<SqlPrimaryKeyColumn> columns)
        {
            this.Name = name;
            this.Type = type;

            this.Columns = new ReadOnlyCollection<SqlPrimaryKeyColumn>(columns);
        }

        /// <summary>
        /// Gets the name of the primary key.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the primary key.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the columns which belong to the primary key.
        /// </summary>
        public ReadOnlyCollection<SqlPrimaryKeyColumn> Columns { get; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
