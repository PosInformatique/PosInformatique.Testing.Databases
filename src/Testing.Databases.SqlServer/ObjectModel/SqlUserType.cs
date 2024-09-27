//-----------------------------------------------------------------------
// <copyright file="SqlUserType.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents a user type in SQL database.
    /// </summary>
    public sealed class SqlUserType : SqlObject
    {
        internal SqlUserType()
        {
        }

        /// <summary>
        /// Gets the name of the user type.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the max length of the type.
        /// </summary>
        public required short MaxLength { get; init; }

        /// <summary>
        /// Gets a value indicating whether is the type is nullable.
        /// </summary>
        public required bool IsNullable { get; init; }

        /// <summary>
        /// Gets a value indicating whether is the type is a table.
        /// </summary>
        public required bool IsTableType { get; init; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
