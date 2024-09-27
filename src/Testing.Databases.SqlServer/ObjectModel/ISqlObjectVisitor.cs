//-----------------------------------------------------------------------
// <copyright file="ISqlObjectVisitor.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    /// <summary>
    /// Used to visit all the <see cref="SqlObject"/>.
    /// </summary>
    /// <typeparam name="TResult">Type of the result returned by the visit.</typeparam>
    public interface ISqlObjectVisitor<TResult>
    {
        /// <summary>
        /// Visits the specified <paramref name="checkConstraint"/>.
        /// </summary>
        /// <param name="checkConstraint"><see cref="SqlCheckConstraint"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlCheckConstraint checkConstraint);

        /// <summary>
        /// Visits the specified <paramref name="column"/>.
        /// </summary>
        /// <param name="column"><see cref="SqlColumn"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlColumn column);

        /// <summary>
        /// Visits the specified <paramref name="foreignKey"/>.
        /// </summary>
        /// <param name="foreignKey"><see cref="SqlForeignKey"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlForeignKey foreignKey);

        /// <summary>
        /// Visits the specified <paramref name="column"/>.
        /// </summary>
        /// <param name="column"><see cref="SqlForeignKeyColumn"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlForeignKeyColumn column);

        /// <summary>
        /// Visits the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index"><see cref="SqlIndex"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlIndex index);

        /// <summary>
        /// Visits the specified <paramref name="column"/>.
        /// </summary>
        /// <param name="column"><see cref="SqlIndexColumn"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlIndexColumn column);

        /// <summary>
        /// Visits the specified <paramref name="primaryKey"/>.
        /// </summary>
        /// <param name="primaryKey"><see cref="SqlPrimaryKey"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlPrimaryKey primaryKey);

        /// <summary>
        /// Visits the specified primary key <paramref name="column"/>.
        /// </summary>
        /// <param name="column"><see cref="SqlPrimaryKeyColumn"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlPrimaryKeyColumn column);

        /// <summary>
        /// Visits the specified <paramref name="storedProcedure"/>.
        /// </summary>
        /// <param name="storedProcedure"><see cref="SqlStoredProcedure"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlStoredProcedure storedProcedure);

        /// <summary>
        /// Visits the specified <paramref name="table"/>.
        /// </summary>
        /// <param name="table"><see cref="SqlTable"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlTable table);

        /// <summary>
        /// Visits the specified <paramref name="trigger"/>.
        /// </summary>
        /// <param name="trigger"><see cref="SqlTrigger"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlTrigger trigger);

        /// <summary>
        /// Visits the specified <paramref name="uniqueConstraint"/>.
        /// </summary>
        /// <param name="uniqueConstraint"><see cref="SqlUniqueConstraint"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlUniqueConstraint uniqueConstraint);

        /// <summary>
        /// Visits the specified <paramref name="userType"/>.
        /// </summary>
        /// <param name="userType"><see cref="SqlUserType"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlUserType userType);

        /// <summary>
        /// Visits the specified <paramref name="view"/>.
        /// </summary>
        /// <param name="view"><see cref="SqlView"/> to visit.</param>
        /// <returns>The result of the visit.</returns>
        TResult Visit(SqlView view);
    }
}
