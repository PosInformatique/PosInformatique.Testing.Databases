//-----------------------------------------------------------------------
// <copyright file="SqlCheckConstraint.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents a check constraint in SQL table.
    /// </summary>
    public sealed class SqlCheckConstraint : SqlObject
    {
        internal SqlCheckConstraint(string name, string code)
        {
            this.Name = name;
            this.Code = code;
        }

        /// <summary>
        /// Gets the name of the check constraint type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the code of the check constraint.
        /// </summary>
        public string Code { get; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
