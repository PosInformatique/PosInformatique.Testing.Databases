//-----------------------------------------------------------------------
// <copyright file="SqlTrigger.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    /// <summary>
    /// Represents a trigger in SQL database.
    /// </summary>
    public sealed class SqlTrigger : SqlObject
    {
        internal SqlTrigger()
        {
        }

        /// <summary>
        /// Gets the name of the trigger.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets a value indicating whether if the trigger is an INSTEAD OF trigger.
        /// </summary>
        public bool IsInsteadOfTrigger { get; init; }

        /// <summary>
        /// Gets the SQL code of the trigger.
        /// </summary>
        public required string Code { get; init; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
