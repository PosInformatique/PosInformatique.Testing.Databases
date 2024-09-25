//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseObjectExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Data;
    using System.Globalization;

    /// <summary>
    /// Contains extension methods to retrieve the schema information of a <see cref="SqlServerDatabase"/>.
    /// </summary>
    public static class SqlServerDatabaseObjectExtensions
    {
        /// <summary>
        /// Gets the stored procedures in the <paramref name="database"/>.
        /// </summary>
        /// <param name="database">SQL Server database which the stored procedures have to be retrieve.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> used to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> which represents the asynchronous operation and contains the list of the stored procedures in the <paramref name="database"/>.</returns>
        public static async Task<IReadOnlyList<SqlStoredProcedure>> GetStoredProceduresAsync(this SqlServerDatabase database, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT
				    [s].[name] AS [Schema],
				    [p].[name] AS [Name],
				    [sm].[definition] AS [Code]
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

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            return result.Rows.Cast<DataRow>().Select(ToStoredProcedure).ToArray();
        }

        /// <summary>
        /// Gets the tables in the <paramref name="database"/>.
        /// </summary>
        /// <param name="database">SQL Server database which the tables have to be retrieve.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> used to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> which represents the asynchronous operation and contains the list of the tables in the <paramref name="database"/>.</returns>
        public static async Task<IReadOnlyList<SqlTable>> GetTablesAsync(this SqlServerDatabase database, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT
                    [t].[object_id] AS [Id],
                    [s].[name] AS [Schema],
                    [t].[name] AS [Name]
                FROM
                    [sys].[schemas] AS [s],
                    [sys].[tables] AS [t]
                WHERE
		            [s].[schema_id] = [t].[schema_id]
                ORDER BY
                    [s].[name],
                    [t].[name]";

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            var tables = new List<SqlTable>(result.Rows.Count);

            // Gets the columns
            var allColumns = GetColumnsAsync(database, cancellationToken);

            // Gets the check constraints
            var allCheckConstraints = GetCheckConstraintsAsync(database, cancellationToken);

            // Gets the indexes
            var allForeignKeys = GetForeignKeysAsync(database, cancellationToken);

            // Gets the indexes
            var allIndexes = GetIndexesAsync(database, cancellationToken);

            // Gets the primary keys
            var allPrimaryKeys = GetPrimaryKeysAsync(database, cancellationToken);

            // Gets the triggers
            var allTriggers = GetTriggersAsync(database, cancellationToken);

            // Gets the unique constraints
            var allUniqueConstraints = GetUniqueConstraintsAsync(database, cancellationToken);

            await Task.WhenAll(allColumns, allCheckConstraints, allForeignKeys, allIndexes, allPrimaryKeys, allTriggers, allUniqueConstraints);

            // Builds the SqlTable object
            foreach (var table in result.AsEnumerable())
            {
                // Check constraints
                var checkConstraintsTable = allCheckConstraints.Result[(int)table["Id"]];
                var checkConstraints = new List<SqlCheckConstraint>();

                foreach (var checkConstraint in checkConstraintsTable.OrderBy(r => r["Name"]))
                {
                    checkConstraints.Add(ToCheckConstraint(checkConstraint));
                }

                // Columns
                var columnsTable = allColumns.Result[(int)table["Id"]];
                var columns = new List<SqlColumn>();

                foreach (var column in columnsTable.OrderBy(r => r["Position"]))
                {
                    columns.Add(ToColumn(column));
                }

                // Indexes
                var foreignKeysTable = allForeignKeys.Result[(int)table["Id"]];
                var foreignKeys = new List<SqlForeignKey>();

                foreach (var foreignKey in foreignKeysTable.GroupBy(r => r["ForeignKeyName"]))
                {
                    var foreignKeyColumns = new List<SqlForeignKeyColumn>();

                    foreach (var column in foreignKey.OrderBy(r => r["Position"]))
                    {
                        foreignKeyColumns.Add(ToForeignKeyColumn(column));
                    }

                    foreignKeys.Add(ToForeignKey(foreignKey.First(), foreignKeyColumns));
                }

                // Indexes
                var indexesTable = allIndexes.Result[(int)table["Id"]];
                var indexes = new List<SqlIndex>();

                foreach (var index in indexesTable.GroupBy(r => r["IndexName"]))
                {
                    var indexColumns = new List<SqlIndexColumn>();
                    var indexIncludesColumns = new List<SqlIndexColumn>();

                    foreach (var column in index.OrderBy(r => r["Position"]))
                    {
                        var isIncluded = (bool)column["IsIncludedColumn"];

                        if (isIncluded)
                        {
                            indexIncludesColumns.Add(ToIndexColumn(column));
                        }
                        else
                        {
                            indexColumns.Add(ToIndexColumn(column));
                        }
                    }

                    indexes.Add(ToIndex(index.First(), indexColumns, indexIncludesColumns));
                }

                // Primary key
                var primaryKeyTable = allPrimaryKeys.Result[(int)table["Id"]];
                var primaryKeyColumns = new List<SqlPrimaryKeyColumn>();

                foreach (var column in primaryKeyTable.OrderBy(r => r["Position"]))
                {
                    primaryKeyColumns.Add(ToPrimaryKeyColumn(column));
                }

                var primaryKey = ToPrimaryKey(primaryKeyTable.FirstOrDefault(), primaryKeyColumns);

                // Triggers
                var triggersTable = allTriggers.Result[(int)table["Id"]];
                var triggers = new List<SqlTrigger>();

                foreach (var trigger in triggersTable)
                {
                    triggers.Add(ToTrigger(trigger));
                }

                // Unique constraints
                var uniqueConstraintsTable = allUniqueConstraints.Result[(int)table["Id"]];
                var uniqueConstraints = new List<SqlUniqueConstraint>();

                foreach (var uniqueConstraint in uniqueConstraintsTable.GroupBy(r => r["ConstraintName"]))
                {
                    var uniqueConstraintColumns = new List<SqlIndexColumn>();

                    foreach (var column in uniqueConstraint.OrderBy(r => r["Position"]))
                    {
                        uniqueConstraintColumns.Add(ToIndexColumn(column));
                    }

                    uniqueConstraints.Add(ToUniqueConstraint(uniqueConstraint.First(), uniqueConstraintColumns));
                }

                // Build and add the table object
                tables.Add(new SqlTable(columns, triggers, checkConstraints, indexes, foreignKeys, uniqueConstraints)
                {
                    Name = (string)table["Name"],
                    PrimaryKey = primaryKey,
                    Schema = (string)table["Schema"],
                });
            }

            return tables;
        }

        /// <summary>
        /// Gets the user types in the <paramref name="database"/>.
        /// </summary>
        /// <param name="database">SQL Server database which the user types have to be retrieve.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> used to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> which represents the asynchronous operation and contains the list of the views in the <paramref name="database"/>.</returns>
        public static async Task<IReadOnlyList<SqlUserType>> GetUserTypesAsync(this SqlServerDatabase database, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT
				    [t].[name] AS [Name],
				    [t].[max_length] AS [MaxLength],
				    [t].[is_nullable] AS [IsNullable],
				    [t].[is_table_type] AS [IsTableType]
			    FROM
				    [sys].[types] AS [t]
			    WHERE
				    [t].[is_user_defined] = 1";

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            return result.Rows.Cast<DataRow>().Select(ToUserType).ToArray();
        }

        /// <summary>
        /// Gets the views in the <paramref name="database"/>.
        /// </summary>
        /// <param name="database">SQL Server database which the views have to be retrieve.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> used to cancel the asynchronous operation.</param>
        /// <returns>The list of the views in the <paramref name="database"/>.</returns>
        public static async Task<IReadOnlyList<SqlView>> GetViewsAsync(this SqlServerDatabase database, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT
				    [s].[name] AS [Schema],
				    [v].[name] AS [Name],
				    [sm].[definition] AS [Code]
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

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            return result.Rows.Cast<DataRow>().Select(ToView).ToArray();
        }

        private static async Task<ILookup<int, DataRow>> GetCheckConstraintsAsync(SqlServerDatabase database, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
				    [t].[object_id] AS [TableId],
				    [c].[name] AS [Name],
				    [c].[definition] AS [Code]
			    FROM
				    [sys].[check_constraints] AS [c],
				    [sys].[tables] AS [t]
			    WHERE
					    [c].[parent_object_id] = [t].[object_id]
				    AND [c].[name] NOT LIKE 'CK__%'		-- Ignore the generated check contraints
                ORDER BY
                    [t].[name],
				    [c].[name]";

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            return result.AsEnumerable().ToLookup(c => (int)c["TableId"]);
        }

        private static async Task<ILookup<int, DataRow>> GetColumnsAsync(SqlServerDatabase database, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
	                [t].[object_id] AS [TableId],
	                [c].[name] AS [Name],
	                ROW_NUMBER() OVER(PARTITION BY [t].[name] ORDER BY [t].[name], [c].[column_id]) AS [Position],
	                [ty].[name] AS [TypeName],
	                [c].[max_length] AS [MaxLength],
	                [c].[precision] AS [Precision],
	                [c].[scale] AS [Scale],
	                [c].[collation_name] AS [CollationName],
	                [c].[is_nullable] AS [IsNullable],
	                [c].[is_identity] AS [IsIdentity],
	                [c].[is_computed] AS [IsComputed],
	                [cc].[definition] AS [ComputedExpression]
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
	                AND [c].[user_type_id] = [ty].[user_type_id]";

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            return result.AsEnumerable().ToLookup(c => (int)c["TableId"]);
        }

        private static async Task<ILookup<int, DataRow>> GetForeignKeysAsync(SqlServerDatabase database, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
                    [t].[object_id] AS [TableId],
                    [fk].[name] AS [ForeignKeyName],
                    [fkc].[constraint_column_id] AS [Position],
                    [pc].[name] AS [ColumnName],
                    [rt].[name] AS [ReferencedTableName],
                    [rc].[name] AS [ReferencedColumnName],
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
                    [t].[object_id], [fk].[name], [fkc].[constraint_column_id]";

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            return result.AsEnumerable().ToLookup(c => (int)c["TableId"]);
        }

        private static async Task<ILookup<int, DataRow>> GetIndexesAsync(SqlServerDatabase database, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
				    [t].[object_id] AS [TableId],
				    [i].[name] AS [IndexName],
				    [c].[name] AS [ColumnName],
				    [ic].[key_ordinal] AS [Position],
				    [i].[type_desc] AS [Type],
				    [i].[is_unique] AS [IsUnique],
				    [ic].[is_included_column] AS [IsIncludedColumn],
				    [i].[filter_definition] AS [Filter]
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
			    ORDER BY [t].[name], [i].[name], [ic].[key_ordinal]";

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            return result.AsEnumerable().ToLookup(c => (int)c["TableId"]);
        }

        private static async Task<ILookup<int, DataRow>> GetPrimaryKeysAsync(SqlServerDatabase database, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
                    [t].[object_id] AS [TableId],
                    [i].[name] AS [Name],
                    [i].[type_desc] AS [Type],
                    [c].[name] AS [ColumnName],
	                [ic].[key_ordinal] AS [Position]
                FROM
                    [sys].[tables] AS [t],
                    [sys].[indexes] AS [i],
                    [sys].[index_columns] AS [ic],
                    [sys].[columns] AS [c]
                WHERE
                        [t].[name] NOT IN ('__EFMigrationsHistory')
                    AND [t].[object_id] = [i].[object_id]
                    AND [i].[is_primary_key] = 1
                    AND [i].[object_id] = [ic].[object_id]
                    AND [i].[index_id] = [ic].[index_id]
                    AND [ic].[column_id] = [c].[column_id]
                    AND [i].[object_id] = [c].[object_id]
                ORDER BY
                    [t].[object_id], [ic].[key_ordinal]";

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            return result.AsEnumerable().ToLookup(c => (int)c["TableId"]);
        }

        private static async Task<ILookup<int, DataRow>> GetUniqueConstraintsAsync(SqlServerDatabase database, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
	                [t].[object_id] AS [TableId],
	                [i].[name] AS [ConstraintName],
	                [i].[type_desc] AS [Type],
				    [ic].[key_ordinal] AS [Position],
	                [c].[name] AS [ColumnName]
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
                ORDER BY
	                [i].[name], [ic].[key_ordinal]";

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            return result.AsEnumerable().ToLookup(c => (int)c["TableId"]);
        }

        private static async Task<ILookup<int, DataRow>> GetTriggersAsync(SqlServerDatabase database, CancellationToken cancellationToken)
        {
            const string sql = @"
	            SELECT
				    [t].[object_id] AS [TableId],
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
                ORDER BY [t].[object_id], [tr].[name]";

            var result = await database.ExecuteQueryAsync(sql, cancellationToken);

            return result.AsEnumerable().ToLookup(c => (int)c["TableId"]);
        }

        private static SqlCheckConstraint ToCheckConstraint(DataRow row)
        {
            return new SqlCheckConstraint()
            {
                Code = (string)row["Code"],
                Name = (string)row["Name"],
            };
        }

        private static SqlColumn ToColumn(DataRow row)
        {
            return new SqlColumn()
            {
                CollationName = NullIfDbNull<string>(row["CollationName"]),
                ComputedExpression = NullIfDbNull<string>(row["ComputedExpression"]),
                IsComputed = (bool)row["IsComputed"],
                IsIdentity = (bool)row["IsIdentity"],
                IsNullable = (bool)row["IsNullable"],
                MaxLength = (short)row["MaxLength"],
                Name = (string)row["Name"],
                Precision = (byte)row["Precision"],
                Position = Convert.ToInt32(row["Position"], CultureInfo.InvariantCulture),
                Scale = (byte)row["Scale"],
                TypeName = (string)row["TypeName"],
            };
        }

        private static SqlForeignKey ToForeignKey(DataRow row, IList<SqlForeignKeyColumn> columns)
        {
            return new SqlForeignKey(columns)
            {
                DeleteAction = (string)row["DeleteAction"],
                Name = (string)row["ForeignKeyName"],
                ReferencedTable = (string)row["ReferencedTableName"],
                UpdateAction = (string)row["UpdateAction"],
            };
        }

        private static SqlForeignKeyColumn ToForeignKeyColumn(DataRow row)
        {
            return new SqlForeignKeyColumn()
            {
                Name = (string)row["ColumnName"],
                Referenced = (string)row["ReferencedColumnName"],
                Position = (int)row["Position"],
            };
        }

        private static SqlIndex ToIndex(DataRow row, IList<SqlIndexColumn> columns, IList<SqlIndexColumn> includedColumn)
        {
            return new SqlIndex(columns, includedColumn)
            {
                Name = (string)row["IndexName"],
                Filter = NullIfDbNull<string>(row["Filter"]),
                IsUnique = (bool)row["IsUnique"],
                Type = (string)row["Type"],
            };
        }

        private static SqlIndexColumn ToIndexColumn(DataRow row)
        {
            return new SqlIndexColumn()
            {
                Name = (string)row["ColumnName"],
                Position = Convert.ToInt32(row["Position"], CultureInfo.InvariantCulture),
            };
        }

        private static SqlPrimaryKey? ToPrimaryKey(DataRow? row, IList<SqlPrimaryKeyColumn> columns)
        {
            if (row == null)
            {
                return null;
            }

            return new SqlPrimaryKey(columns)
            {
                Name = (string)row["Name"],
                Type = (string)row["Type"],
            };
        }

        private static SqlPrimaryKeyColumn ToPrimaryKeyColumn(DataRow column)
        {
            return new SqlPrimaryKeyColumn()
            {
                Name = (string)column["ColumnName"],
                Position = (byte)column["Position"],
            };
        }

        private static SqlStoredProcedure ToStoredProcedure(DataRow row)
        {
            return new SqlStoredProcedure()
            {
                Code = (string)row["Code"],
                Name = (string)row["Name"],
                Schema = (string)row["Schema"],
            };
        }

        private static SqlTrigger ToTrigger(DataRow row)
        {
            return new SqlTrigger()
            {
                Code = (string)row["Code"],
                IsInsteadOfTrigger = (bool)row["IsInsteadOfTrigger"],
                Name = (string)row["Name"],
            };
        }

        private static SqlUniqueConstraint ToUniqueConstraint(DataRow row, IList<SqlIndexColumn> columns)
        {
            return new SqlUniqueConstraint(columns)
            {
                Name = (string)row["ConstraintName"],
                Type = (string)row["Type"],
            };
        }

        private static SqlUserType ToUserType(DataRow row)
        {
            return new SqlUserType()
            {
                IsNullable = (bool)row["IsNullable"],
                IsTableType = (bool)row["IsTableType"],
                Name = (string)row["Name"],
                MaxLength = (short)row["MaxLength"],
            };
        }

        private static SqlView ToView(DataRow row)
        {
            return new SqlView()
            {
                Code = (string)row["Code"],
                Name = (string)row["Name"],
                Schema = (string)row["Schema"],
            };
        }

        private static TValue? NullIfDbNull<TValue>(object value)
            where TValue : class
        {
            if (value == DBNull.Value)
            {
                return null;
            }

            return (TValue)value;
        }
    }
}
