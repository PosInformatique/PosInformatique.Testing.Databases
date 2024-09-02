//-----------------------------------------------------------------------
// <copyright file="SqlServerUniqueConstraintsComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal sealed class SqlServerUniqueConstraintsComparer : SqlServerObjectComparer
    {
        private const string Sql = @"
            SELECT
	            [t].[name] AS [TableName],
	            [i].[name] AS [ConstraintName],
	            [i].[type_desc] AS [Type],
	            [c].[name] AS [ColumnName],
				[c].[column_id] AS [Position]
            FROM
	            [sys].[tables] AS [t],
	            [sys].[indexes] AS [i],
	            [sys].[index_columns] AS [ic],
	            [sys].[columns] AS [c]
            WHERE
		            [t].[name] NOT IN ('__EFMigrationsHistory')
	            AND [t].[object_id] = [i].[object_id]
	            AND [i].[is_unique_constraint] = 1
	            AND [i].[object_id] = [ic].[object_id]
	            AND [i].[index_id] = [ic].[index_id]
	            AND [ic].[column_id] = [c].[column_id]
	            AND [i].[object_id] = [c].[object_id]
				AND [i].[name] NOT LIKE 'UQ__%'		-- Ignore the generated unique contraints
            ORDER BY
	            [i].[name], [ic].[key_ordinal]";

        public SqlServerUniqueConstraintsComparer()
            : base("UniqueConstraints", Sql, ["TableName", "ConstraintName", "ColumnName"])
        {
        }
    }
}
