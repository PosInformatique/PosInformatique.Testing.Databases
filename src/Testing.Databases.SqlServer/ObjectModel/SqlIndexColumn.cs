//-----------------------------------------------------------------------
// <copyright file="SqlIndexColumn.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents a column of an index.
    /// </summary>
    public sealed class SqlIndexColumn : SqlObject
    {
        internal SqlIndexColumn(string name, int position)
        {
            this.Name = name;
            this.Position = position;
        }

        /// <summary>
        /// Gets the name of the column of the index.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the position of the column of the index.
        /// </summary>
        public int Position { get; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
