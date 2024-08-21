//-----------------------------------------------------------------------
// <copyright file="SqlServerCheckConstraintsComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal sealed class SqlServerCheckConstraintsComparer : SqlServerObjectComparer
    {
        private const string Sql = @"
            SELECT
				[t].[name] AS [TableName],
				[c].[name] AS [Name],
				[c].[definition] AS [Definition]
			FROM
				[sys].[check_constraints] AS [c],
				[sys].[tables] AS [t]
			WHERE
					[c].[parent_object_id] = [t].[object_id]
				AND [c].[name] NOT LIKE 'CK__%'		-- Ignore the generated check contraints
            ORDER BY
                [t].[name],
				[c].[name]";

        public SqlServerCheckConstraintsComparer()
            : base("CheckConstraints", Sql, ["Name"])
        {
        }
    }
}
