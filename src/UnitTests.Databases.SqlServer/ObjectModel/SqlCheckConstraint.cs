//-----------------------------------------------------------------------
// <copyright file="SqlCheckConstraint.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    /// <summary>
    /// Represents a check constraint in SQL table.
    /// </summary>
    public sealed class SqlCheckConstraint : SqlObject
    {
        internal SqlCheckConstraint()
        {
        }

        /// <summary>
        /// Gets the name of the check constraint type.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the code of the check constraint.
        /// </summary>
        public required string Code { get; init; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);
    }
}
