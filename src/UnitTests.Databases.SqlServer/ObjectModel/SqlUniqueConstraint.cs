//-----------------------------------------------------------------------
// <copyright file="SqlUniqueConstraint.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents an unique constraint of a SQL table.
    /// </summary>
    public sealed class SqlUniqueConstraint : SqlObject
    {
        internal SqlUniqueConstraint(IList<SqlIndexColumn> columns)
        {
            this.Columns = new ReadOnlyCollection<SqlIndexColumn>(columns);
        }

        /// <summary>
        /// Gets the name of the unique constraint.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the columns of the unique constraint.
        /// </summary>
        public ReadOnlyCollection<SqlIndexColumn> Columns { get; }

        /// <summary>
        /// Gets the type of the unique constraint.
        /// </summary>
        public required string Type { get; init; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
