//-----------------------------------------------------------------------
// <copyright file="SqlServerObjectComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Data;

    internal abstract class SqlServerObjectComparer
    {
        private readonly string name;

        private readonly string sql;

        private readonly string[] primaryKeyColumns;

        protected SqlServerObjectComparer(string name, string sql, string[] primaryKeyColumns)
        {
            this.name = name;
            this.sql = sql;
            this.primaryKeyColumns = primaryKeyColumns;
        }

        public SqlDatabaseObjectDifferences Compare(SqlServerDatabase source, SqlServerDatabase target)
        {
            var sourceTable = source.ExecuteQuery(this.sql);
            var targetTable = target.ExecuteQuery(this.sql);

            // Add PK in the table to search columns by name
            sourceTable.PrimaryKey = this.primaryKeyColumns.Select(column => sourceTable.Columns[column]!).ToArray();
            targetTable.PrimaryKey = this.primaryKeyColumns.Select(column => targetTable.Columns[column]!).ToArray();

            // Initalize the results
            var differences = new List<SqlDatabaseObjectDifference>();

            // Iterate on the target
            foreach (var targetRow in targetTable.AsEnumerable())
            {
                var keyValue = this.primaryKeyColumns.Select(column => targetRow[column]).ToArray();
                var sourceRow = sourceTable.Rows.Find(keyValue);

                if (sourceRow is null)
                {
                    differences.Add(new SqlDatabaseObjectDifference(null, targetRow, SqlDatabaseObjectDifferenceType.MissingInSource, keyValue));
                }
                else
                {
                    // Compare the rows
                    var difference = Compare(sourceRow, targetRow, keyValue);

                    if (difference is not null)
                    {
                        differences.Add(difference);
                    }
                }
            }

            // Iterate on the source
            foreach (var sourceRow in sourceTable.AsEnumerable())
            {
                var keyValue = this.primaryKeyColumns.Select(column => sourceRow[column]).ToArray();
                var targetRow = targetTable.Rows.Find(keyValue);

                if (targetRow is null)
                {
                    differences.Add(new SqlDatabaseObjectDifference(sourceRow, null, SqlDatabaseObjectDifferenceType.MissingInTarget, keyValue));
                }
            }

            return new SqlDatabaseObjectDifferences(this.name, differences);
        }

        private static SqlDatabaseObjectDifference? Compare(DataRow row, DataRow target, object[] keyValue)
        {
            for (var i = 0; i < target.Table.Columns.Count; i++)
            {
                if (!Equals(target[i], row[i]))
                {
                    return new SqlDatabaseObjectDifference(target, row, SqlDatabaseObjectDifferenceType.Different, keyValue);
                }
            }

            return null;
        }
    }
}
