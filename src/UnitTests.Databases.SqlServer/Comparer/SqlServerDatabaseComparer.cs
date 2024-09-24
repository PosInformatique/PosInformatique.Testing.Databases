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
    public class SqlServerDatabaseComparer
    {
        /// <summary>
        /// Compares two database and returns the differences found.
        /// </summary>
        /// <param name="source">First database to compare with <paramref name="target"/>.</param>
        /// <param name="target">Second database to compare with <paramref name="source"/>.</param>
        /// <returns>The difference between the two databases.</returns>
        public SqlDatabaseComparisonResults Compare(SqlServerDatabase source, SqlServerDatabase target)
        {
            // Compares the stored procedures
            var sourceStoredProcedures = source.GetStoredProcedures();
            var targetStoredProcedures = target.GetStoredProcedures();

            var storedProceduresDifferences = SqlObjectComparer.Compare(sourceStoredProcedures, targetStoredProcedures, sp => sp.Schema + "." + sp.Name);

            // Compares the tables
            var sourceTables = source.GetTables();
            var targetTables = target.GetTables();

            var tablesDifferences = SqlObjectComparer.Compare(sourceTables, targetTables);

            // Compares the user types
            var sourceUserTypes = source.GetUserTypes();
            var targetUserTypes = target.GetUserTypes();

            var userTypesDifferences = SqlObjectComparer.Compare(sourceUserTypes, targetUserTypes, ut => ut.Name);

            // Compares the views
            var sourceViews = source.GetViews();
            var targetViews = target.GetViews();

            var viewsDifferences = SqlObjectComparer.Compare(sourceViews, targetViews, v => v.Schema + "." + v.Name);

            return new SqlDatabaseComparisonResults()
            {
                StoredProcedures = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlStoredProcedure>>(storedProceduresDifferences),
                Tables = new ReadOnlyCollection<SqlDatabaseTableDifferences>(tablesDifferences),
                UserTypes = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlUserType>>(userTypesDifferences),
                Views = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlView>>(viewsDifferences),
            };
        }
    }
}
