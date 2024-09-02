//-----------------------------------------------------------------------
// <copyright file="SqlServerTriggersComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal sealed class SqlServerTriggersComparer : SqlServerObjectComparer
    {
        private const string Sql = @"
            SELECT
				[t].[name] AS [TableName],
				[tr].[name] AS [Name],
				[tr].[is_instead_of_trigger] AS [IsInsteadOfTrigger],
				[c].[text] AS [Code]
			FROM
				[sys].[triggers] AS [tr],
				[sys].[tables] AS [t],
				[sys].[syscomments] AS [c]
			WHERE
					[tr].[parent_id] = [t].[object_id]
				AND [tr].[object_id] = [c].[id]
            ORDER BY [t].[name], [tr].[name]";

        public SqlServerTriggersComparer()
            : base("Triggers", Sql, ["TableName", "Name"])
        {
        }

        protected override bool AreEqual(object source, object target, string columnName)
        {
            if (columnName == "Code")
            {
                return TsqlCodeHelper.AreEqual((string)source, (string)target);
            }

            return base.AreEqual(source, target, columnName);
        }
    }
}
