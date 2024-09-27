//-----------------------------------------------------------------------
// <copyright file="SqlColumn.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents a column of a table in SQL database.
    /// </summary>
    public sealed class SqlColumn : SqlObject
    {
        internal SqlColumn()
        {
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the position of the column in the database.
        /// </summary>
        public required int Position { get; init; }

        /// <summary>
        /// Gets the type name of the column.
        /// </summary>
        public required string TypeName { get; init; }

        /// <summary>
        /// Gets the max length of the column.
        /// </summary>
        public required short MaxLength { get; init; }

        /// <summary>
        /// Gets the precision of the column.
        /// </summary>
        public required byte Precision { get; init; }

        /// <summary>
        /// Gets the scale of the column.
        /// </summary>
        public required byte Scale { get; init; }

        /// <summary>
        /// Gets the collation name of the column.
        /// </summary>
        public required string? CollationName { get; init; }

        /// <summary>
        /// Gets a value indicating whether if the column is nullable.
        /// </summary>
        public required bool IsNullable { get; init; }

        /// <summary>
        /// Gets a value indicating whether if the column is identity.
        /// </summary>
        public required bool IsIdentity { get; init; }

        /// <summary>
        /// Gets a value indicating whether if the column is computed.
        /// </summary>
        public required bool IsComputed { get; init; }

        /// <summary>
        /// Gets the computed expression of the column.
        /// </summary>
        public required string? ComputedExpression { get; init; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
