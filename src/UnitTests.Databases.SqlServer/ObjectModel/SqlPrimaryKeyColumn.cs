//-----------------------------------------------------------------------
// <copyright file="SqlPrimaryKeyColumn.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    /// <summary>
    /// Represents a column of a primary key.
    /// </summary>
    public sealed class SqlPrimaryKeyColumn : SqlObject
    {
        internal SqlPrimaryKeyColumn()
        {
        }

        /// <summary>
        /// Gets the name of the column of the primary key.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the position of the column of the primary key.
        /// </summary>
        public required byte Position { get; init; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);
    }
}
