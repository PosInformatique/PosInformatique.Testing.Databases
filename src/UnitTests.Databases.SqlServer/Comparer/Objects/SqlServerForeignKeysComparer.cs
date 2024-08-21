//-----------------------------------------------------------------------
// <copyright file="SqlServerForeignKeysComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal sealed class SqlServerForeignKeysComparer : SqlServerObjectComparer
    {
        private const string Sql = @"
            SELECT
	            [fk].[name] AS [ForeignKeyName],
	            [t].[name] AS [TableName],
	            [pc].[name] AS [ColumnName],
	            [rt].[name] AS [ReferencedColumnName],
	            [rc].[name] AS [ReferencedTableName],
	            [fk].[delete_referential_action_desc] AS [DeleteAction],
	            [fk].[update_referential_action_desc] AS [UpdateAction]
            FROM
	            [sys].[tables] AS [t],
	            [sys].[foreign_keys] AS [fk],
	            [sys].[foreign_key_columns] AS [fkc],
	            [sys].[columns] AS [pc],
	            [sys].[columns] AS [rc],
	            [sys].[tables] AS [rt]
            WHERE
		            [t].[name] NOT IN ('__EFMigrationsHistory')
	            AND [t].[object_id] = [fk].[parent_object_id]
	            AND [fk].[object_id] = [fkc].[constraint_object_id]
	            AND [fkc].[parent_column_id] = [pc].[column_id]
	            AND [pc].[object_id] = [t].[object_id]
	            AND [fkc].[referenced_column_id] = [rc].[column_id]
	            AND [rc].[object_id] = [rt].[object_id]
	            AND [fk].[referenced_object_id] = [rt].[object_id]
            ORDER BY
	            [fk].[name], [pc].[column_id]";

        public SqlServerForeignKeysComparer()
            : base("ForeignKeys", Sql, ["ForeignKeyName"])
        {
        }
    }
}
