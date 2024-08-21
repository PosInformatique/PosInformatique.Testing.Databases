//-----------------------------------------------------------------------
// <copyright file="SqlServerColumnsComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal sealed class SqlServerColumnsComparer : SqlServerObjectComparer
    {
        private const string Sql = @"
            SELECT
				[t].[name] AS [TableName],
				[c].[name] AS [ColumnName],
				[c].[column_id] AS [Position],
				[c].[system_type_id] AS [SystemTypeId],
				[ty].[name] AS [TypeName],
				[c].[max_length] AS [MaxLength],
				[c].[precision] AS [Precision],
				[c].[scale] AS [Scale],
				[c].[collation_name] AS [CollationName],
				[c].[is_nullable] AS [IsNullable],
				[c].[is_identity] AS [IsIdentity],
				[c].[is_computed] AS [IsComputed],
				REPLACE(REPLACE(REPLACE(REPLACE([cc].[definition], ' ', ''), CHAR(9), ''), CHAR(10), ''), CHAR(13), '') AS [ComputedExpression]
			FROM
				[sys].[columns] AS [c]
					LEFT OUTER JOIN
				[sys].[computed_columns] AS [cc]
					ON ([c].[object_id] = [cc].[object_id] AND [c].[column_id] = [cc].[column_id]),
				[sys].[tables] AS [t],
				[sys].[types] AS [ty]
			WHERE
					[t].[name] NOT IN ('__EFMigrationsHistory')
				AND [t].[object_id] = [c].[object_id]
				AND [c].[user_type_id] = [ty].[user_type_id]
			ORDER BY
				[t].[name], [c].[column_id]";

        public SqlServerColumnsComparer()
            : base("Columns", Sql, ["TableName", "ColumnName"])
        {
        }
    }
}
