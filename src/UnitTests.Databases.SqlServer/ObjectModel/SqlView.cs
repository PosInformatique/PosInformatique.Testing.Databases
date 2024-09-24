//-----------------------------------------------------------------------
// <copyright file="SqlView.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    /// <summary>
    /// Represents a view in SQL database.
    /// </summary>
    public sealed class SqlView : SqlObject
    {
        internal SqlView()
        {
        }

        /// <summary>
        /// Gets the schema which the view belong to.
        /// </summary>
        public required string Schema { get; init; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the SQL code of the view.
        /// </summary>
        public required string Code { get; init; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return $"{this.Schema}.{this.Name}";
        }
    }
}
