//-----------------------------------------------------------------------
// <copyright file="SqlForeignKeyColumn.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents a column of a foreign key.
    /// </summary>
    public sealed class SqlForeignKeyColumn : SqlObject
    {
        internal SqlForeignKeyColumn(string name, int position, string referenced)
        {
            this.Name = name;
            this.Position = position;
            this.Referenced = referenced;
        }

        /// <summary>
        /// Gets the name of the column of the foreign key.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the name of the column referenced in the referenced table of the foreign key.
        /// </summary>
        public string Referenced { get; }

        /// <summary>
        /// Gets the position of the column of the foreign key.
        /// </summary>
        public int Position { get; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return $"{this.Name} => {this.Referenced}";
        }
    }
}
