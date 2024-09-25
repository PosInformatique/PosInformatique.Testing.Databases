//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Allows to compare schema difference between 2 databases.
    /// </summary>
    public static class SqlServerDatabaseComparer
    {
        /// <summary>
        /// Compares two database and returns the differences found.
        /// </summary>
        /// <param name="source">First database to compare with <paramref name="target"/>.</param>
        /// <param name="target">Second database to compare with <paramref name="source"/>.</param>
        /// <param name="options">Options of the comparison between the two database.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> used to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> which represents the asynchronous operation and contains the difference between the two databases.</returns>
        public static async Task<SqlDatabaseComparisonResults> CompareAsync(SqlServerDatabase source, SqlServerDatabase target, SqlDatabaseComparerOptions? options = null, CancellationToken cancellationToken = default)
        {
            if (options == null)
            {
                options = new SqlDatabaseComparerOptions();
            }

            // Gets the stored procedures
            var sourceStoredProcedures = source.GetStoredProceduresAsync(cancellationToken);
            var targetStoredProcedures = target.GetStoredProceduresAsync(cancellationToken);

            // Gets the tables
            var sourceTablesTask = source.GetTablesAsync(cancellationToken);
            var targetTablesTask = target.GetTablesAsync(cancellationToken);

            // Gets the user types
            var sourceUserTypes = source.GetUserTypesAsync(cancellationToken);
            var targetUserTypes = target.GetUserTypesAsync(cancellationToken);

            // Gets the views
            var sourceViews = source.GetViewsAsync(cancellationToken);
            var targetViews = target.GetViewsAsync(cancellationToken);

            await Task.WhenAll(
                sourceStoredProcedures,
                targetStoredProcedures,
                sourceTablesTask,
                targetTablesTask,
                sourceUserTypes,
                targetUserTypes,
                sourceViews,
                targetViews);

            var sourceTables = sourceTablesTask.Result.Where(t => !options.ExcludedTables.Contains(t.Name)).ToArray();
            var targetTables = targetTablesTask.Result.Where(t => !options.ExcludedTables.Contains(t.Name)).ToArray();

            var storedProceduresDifferences = SqlObjectComparer.Compare(sourceStoredProcedures.Result, targetStoredProcedures.Result, sp => sp.Schema + "." + sp.Name);
            var tablesDifferences = SqlObjectComparer.Compare(sourceTables, targetTables);
            var userTypesDifferences = SqlObjectComparer.Compare(sourceUserTypes.Result, targetUserTypes.Result, ut => ut.Name);
            var viewsDifferences = SqlObjectComparer.Compare(sourceViews.Result, targetViews.Result, v => v.Schema + "." + v.Name);

            return new SqlDatabaseComparisonResults()
            {
                StoredProcedures = new ReadOnlyCollection<SqlObjectDifferences<SqlStoredProcedure>>(storedProceduresDifferences),
                Tables = new ReadOnlyCollection<SqlTableDifferences>(tablesDifferences),
                UserTypes = new ReadOnlyCollection<SqlObjectDifferences<SqlUserType>>(userTypesDifferences),
                Views = new ReadOnlyCollection<SqlObjectDifferences<SqlView>>(viewsDifferences),
            };
        }
    }
}
