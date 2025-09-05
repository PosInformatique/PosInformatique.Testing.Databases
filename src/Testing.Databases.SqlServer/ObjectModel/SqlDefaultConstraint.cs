//-----------------------------------------------------------------------
// <copyright file="SqlDefaultConstraint.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents a default constraint of a <see cref="SqlColumn" />.
    /// </summary>
    public class SqlDefaultConstraint : SqlObject
    {
        internal SqlDefaultConstraint(string name, string expression)
        {
            this.Name = name;
            this.Expression = expression;
        }

        /// <summary>
        /// Gets the name of the default constraint.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the expression of the default constraint.
        /// </summary>
        public string Expression { get; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
