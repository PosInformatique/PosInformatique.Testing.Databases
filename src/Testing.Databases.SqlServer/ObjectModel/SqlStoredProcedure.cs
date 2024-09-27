//-----------------------------------------------------------------------
// <copyright file="SqlStoredProcedure.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents a stored procedure in SQL database.
    /// </summary>
    public sealed class SqlStoredProcedure : SqlObject
    {
        internal SqlStoredProcedure()
        {
        }

        /// <summary>
        /// Gets the schema which the stored procedure belong to.
        /// </summary>
        public required string Schema { get; init; }

        /// <summary>
        /// Gets the name of the stored procedure.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets the SQL code of the stored procedure.
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
