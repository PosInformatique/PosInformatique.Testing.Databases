//-----------------------------------------------------------------------
// <copyright file="SqlServerTypesComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal sealed class SqlServerTypesComparer : SqlServerObjectComparer
    {
        private const string Sql = @"
            SELECT
				[t].[name] AS [Name],
				[t].[system_type_id] AS [SystemTypeId],
				[t].[max_length] AS [MaxLength],
				[t].[is_nullable] AS [Nullable],
				[t].[is_table_type] AS [IsTableType]
			FROM
				[sys].[types] AS [t]
			WHERE
				[t].[is_user_defined] = 1";

        public SqlServerTypesComparer()
            : base("Types", Sql, ["Name"])
        {
        }
    }
}
