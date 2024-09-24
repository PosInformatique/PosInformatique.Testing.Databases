//-----------------------------------------------------------------------
// <copyright file="SqlIndexColumn.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    /// <summary>
    /// Represents a column of an index.
    /// </summary>
    public sealed class SqlIndexColumn : SqlObject
    {
        internal SqlIndexColumn()
        {
        }

        /// <summary>
        /// Gets the name of the column of the index.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the position of the column of the index.
        /// </summary>
        public required int Position { get; init; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);
    }
}
