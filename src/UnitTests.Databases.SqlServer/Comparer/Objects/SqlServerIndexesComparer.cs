//-----------------------------------------------------------------------
// <copyright file="SqlServerIndexesComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal sealed class SqlServerIndexesComparer : SqlServerObjectComparer
    {
        private const string Sql = @"
            SELECT
				[t].[name] AS [TableName],
				[i].[name] AS [IndexName],
				[c].[name] AS [ColumnName],
				[ic].[index_column_id] AS [Position],
				[i].[type_desc] AS [Type],
				[i].[is_unique] AS [IsUnique],
				[ic].[is_included_column] AS [IsIncludedColumn],
				REPLACE(REPLACE(REPLACE(REPLACE([i].[filter_definition], ' ', ''), CHAR(9), ''), CHAR(10), ''), CHAR(13), '') AS [FilterDefinition]
			FROM
				[sys].[indexes] AS [i],
				[sys].[tables] AS [t],
				[sys].[index_columns] AS [ic],
				[sys].[columns] AS [c]
			WHERE
		            [t].[name] NOT IN ('__EFMigrationsHistory')
				AND [t].[object_id] = [i].[object_id]
				AND [i].[is_unique_constraint] = 0
				AND [i].[object_id] = [ic].[object_id]
				AND [i].[index_id] = [ic].[index_id]
				AND [ic].[column_id] = [c].[column_id]
				AND [ic].[object_id] = [c].[object_id]
			ORDER BY [t].[name], [i].[name], [ic].[index_column_id]";

        public SqlServerIndexesComparer()
            : base("Indexes", Sql, ["TableName", "IndexName", "ColumnName"])
        {
        }
    }
}
