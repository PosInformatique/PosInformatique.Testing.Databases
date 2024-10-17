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
        internal SqlColumn(
            string name,
            int position,
            string typeName,
            short maxLength,
            byte precision,
            byte scale)
        {
            this.Name = name;
            this.Position = position;
            this.TypeName = typeName;
            this.MaxLength = maxLength;
            this.Precision = precision;
            this.Scale = scale;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the position of the column in the database.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Gets the type name of the column.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Gets the max length of the column.
        /// </summary>
        public short MaxLength { get; }

        /// <summary>
        /// Gets the precision of the column.
        /// </summary>
        public byte Precision { get; }

        /// <summary>
        /// Gets the scale of the column.
        /// </summary>
        public byte Scale { get; }

        /// <summary>
        /// Gets the collation name of the column.
        /// </summary>
        public string? CollationName { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether if the column is nullable.
        /// </summary>
        public bool IsNullable { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether if the column is identity.
        /// </summary>
        public bool IsIdentity { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether if the column is computed.
        /// </summary>
        public bool IsComputed { get; internal set; }

        /// <summary>
        /// Gets the computed expression of the column.
        /// </summary>
        public string? ComputedExpression { get; internal set; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
