//-----------------------------------------------------------------------
// <copyright file="SqlPrimaryKeyColumn.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents a column of a primary key.
    /// </summary>
    public sealed class SqlPrimaryKeyColumn : SqlObject
    {
        internal SqlPrimaryKeyColumn(string name, byte position)
        {
            this.Name = name;
            this.Position = position;
        }

        /// <summary>
        /// Gets the name of the column of the primary key.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the position of the column of the primary key.
        /// </summary>
        public byte Position { get; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
