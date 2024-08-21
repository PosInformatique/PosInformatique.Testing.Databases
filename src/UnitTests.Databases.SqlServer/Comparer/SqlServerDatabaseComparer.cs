//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
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

        public SqlServerDatabaseComparer()
        {
        }

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
