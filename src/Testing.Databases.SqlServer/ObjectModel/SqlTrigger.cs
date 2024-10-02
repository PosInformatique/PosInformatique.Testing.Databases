//-----------------------------------------------------------------------
// <copyright file="SqlTrigger.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents a trigger in SQL database.
    /// </summary>
    public sealed class SqlTrigger : SqlObject
    {
        internal SqlTrigger(string name, string code)
        {
            this.Name = name;
            this.Code = code;
        }

        /// <summary>
        /// Gets the name of the trigger.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether if the trigger is an INSTEAD OF trigger.
        /// </summary>
        public bool IsInsteadOfTrigger { get; internal set; }

        /// <summary>
        /// Gets the SQL code of the trigger.
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
