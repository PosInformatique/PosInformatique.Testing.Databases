//-----------------------------------------------------------------------
// <copyright file="SqlForeignKeyColumn.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    /// <summary>
    /// Represents a column of a foreign key.
    /// </summary>
    public sealed class SqlForeignKeyColumn : SqlObject
    {
        internal SqlForeignKeyColumn()
        {
        }

        /// <summary>
        /// Gets the name of the column of the foreign key.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the name of the column referenced in the referenced table of the foreign key.
        /// </summary>
        public required string Referenced { get; init; }

        /// <summary>
        /// Gets the position of the column of the foreign key.
        /// </summary>
        public required int Position { get; init; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);
    }
}
