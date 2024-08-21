//-----------------------------------------------------------------------
// <copyright file="SqlServerViewsComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal sealed class SqlServerViewsComparer : SqlServerObjectComparer
    {
        private const string Sql = @"
            SELECT
				[s].[name] AS [Schema],
				[v].[name] AS [Name],
				REPLACE(REPLACE(REPLACE(REPLACE([sm].[definition], ' ', ''), CHAR(9), ''), CHAR(10), ''), CHAR(13), '') AS [definition]
			FROM
				[sys].[schemas] AS [s],
				[sys].[views] AS [v],
				[sys].[sql_modules] AS [sm]
			WHERE
					[s].[schema_id] = [v].[schema_id]
				AND [v].[object_id] = [sm].[object_id]
			ORDER BY
                [s].[name],
				[v].[name]";

        public SqlServerViewsComparer()
            : base("Views", Sql, ["Schema", "Name"])
        {
        }
    }
}
