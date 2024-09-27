//-----------------------------------------------------------------------
// <copyright file="SqlObject.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Common class to represent a SQL object (Table, Stored procedure,...)
    /// </summary>
    public abstract class SqlObject
    {
        private protected SqlObject()
        {
        }

        /// <summary>
        /// Visit the current <see cref="SqlObject"/> using the specified <paramref name="visitor"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of the result of the visit.</typeparam>
        /// <param name="visitor"><see cref="ISqlObjectVisitor{TResult}"/> which contains the visit methods to call for the current instance.</param>
        /// <returns>The result of the visit.</returns>
        public abstract TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor);
    }
}
