//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    /// <summary>
    /// Allows to compare schema difference between 2 databases.
    /// </summary>
    public class SqlServerDatabaseComparer
    {
        private static readonly SqlServerObjectComparer[] Comparers =
        [
            new SqlServerTypesComparer(),
            new SqlServerColumnsComparer(),
            new SqlServerPrimaryKeysComparer(),
            new SqlServerUniqueConstraintsComparer(),
            new SqlServerForeignKeysComparer(),
            new SqlServerIndexesComparer(),
            new SqlServerCheckConstraintsComparer(),
            new SqlServerStoredProcedureComparer(),
            new SqlServerTriggersComparer(),
            new SqlServerViewsComparer(),
        ];

        /// <summary>
        /// Compares two database and returns the differences found.
        /// </summary>
        /// <param name="source">First database to compare with <paramref name="target"/>.</param>
        /// <param name="target">Second database to compare with <paramref name="source"/>.</param>
        /// <returns>The difference between the two databases.</returns>
        public SqlDatabaseComparisonResults Compare(SqlServerDatabase source, SqlServerDatabase target)
        {
            var differences = new List<SqlDatabaseObjectDifferences>();

            foreach (var comparer in Comparers)
            {
                var objectDifferences = comparer.Compare(source, target);

                if (objectDifferences.Differences.Count > 0)
                {
                    differences.Add(objectDifferences);
                }
            }

            return new SqlDatabaseComparisonResults(differences);
        }
    }
}
