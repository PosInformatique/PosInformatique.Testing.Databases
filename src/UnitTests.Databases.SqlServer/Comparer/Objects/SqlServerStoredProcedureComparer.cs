//-----------------------------------------------------------------------
// <copyright file="SqlServerStoredProcedureComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal sealed class SqlServerStoredProcedureComparer : SqlServerObjectComparer
    {
        private const string Sql = @"
            SELECT
				[s].[name] AS [Schema],
				[p].[name] AS [Name],
				REPLACE(REPLACE(REPLACE(REPLACE([sm].[definition], ' ', ''), CHAR(9), ''), CHAR(10), ''), CHAR(13), '') AS [definition]
			FROM
				[sys].[schemas] AS [s],
				[sys].[procedures] AS [p],
				[sys].[sql_modules] AS [sm]
			WHERE
					[s].[schema_id] = [p].[schema_id]
				AND [p].[object_id] = [sm].[object_id]
			ORDER BY
				[s].[name],
				[p].[name]";

        public SqlServerStoredProcedureComparer()
            : base("StoredProcedures", Sql, ["Schema", "Name"])
        {
        }
    }
}
