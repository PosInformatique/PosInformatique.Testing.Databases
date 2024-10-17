//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Contains extensions methods on the <see cref="SqlServerDatabase"/> class.
    /// </summary>
    public static class SqlServerDatabaseExtensions
    {
        private const string Tab = "  ";

        private static readonly Type[] AuthorizedNonStringTypes =
        [
            typeof(int),
            typeof(double),
            typeof(decimal),
            typeof(int?),
            typeof(double?),
            typeof(decimal?),
        ];

        /// <summary>
        /// Insert data into the table specified by the <paramref name="tableName"/> argument. The row
        /// to insert are represents by objects (or anonymous objects) which the property name must match the
        /// the column name.
        /// </summary>
        /// <typeparam name="T">Type of the object which contains the data to insert in the table.</typeparam>
        /// <param name="database">SQL Server database which contains the table where the data will be inserted.</param>
        /// <param name="tableName">SQL table where the data will be inserted.</param>
        /// <param name="objects">Set of object which represents the row to insert. Each object must have property which are mapped to the column to insert.</param>
        /// <returns>The number of the rows inserted.</returns>
        public static int InsertInto<T>(this SqlServerDatabase database, string tableName, params T[] objects)
        {
            return InsertInto(database, tableName, false, objects);
        }

        /// <summary>
        /// Insert data into the table specified by the <paramref name="tableName"/> argument. The row
        /// to insert are represents by objects (or anonymous objects) which the property name must match the
        /// the column name.
        /// </summary>
        /// <typeparam name="T">Type of the object which contains the data to insert in the table.</typeparam>
        /// <param name="database">SQL Server database which contains the table where the data will be inserted.</param>
        /// <param name="tableName">SQL table where the data will be inserted.</param>
        /// <param name="disableIdentityInsert"><see langword="true"/> to disable auto incrementation of the <c>IDENTITY</c> column. In this case, the object must contains explicitely the value of the <c>IDENTITY</c>
        /// column to insert.</param>
        /// <param name="objects">Set of object which represents the row to insert. Each object must have property which are mapped to the column to insert.</param>
        /// <returns>The number of the rows inserted.</returns>
        public static int InsertInto<T>(this SqlServerDatabase database, string tableName, bool disableIdentityInsert, params T[] objects)
        {
            var builder = new SqlInsertStatementBuilder(tableName);
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                builder.AddColumn(property.Name);
            }

            foreach (var @object in objects)
            {
                foreach (var property in properties)
                {
                    _ = property.PropertyType switch
                    {
                        _ when property.GetValue(@object) == null => builder.AddValue("NULL"),
                        _ when property.PropertyType == typeof(bool) => builder.AddValue(Convert.ToString(Convert.ToInt32((bool)property.GetValue(@object)!), CultureInfo.InvariantCulture)!),
                        _ when property.PropertyType == typeof(bool?) => builder.AddValue(Convert.ToString(Convert.ToInt32(((bool?)property.GetValue(@object)!).Value), CultureInfo.InvariantCulture)!),
                        _ when property.PropertyType == typeof(byte[]) => builder.AddValue((byte[])property.GetValue(@object)!),
                        Type t when Array.Exists(AuthorizedNonStringTypes, at => at == t) => builder.AddValue(Convert.ToString(property.GetValue(@object), CultureInfo.InvariantCulture)!),
                        _ => builder.AddValueWithQuotes((string)property.GetValue(@object)!),
                    };
                }

                if (!@object!.Equals(objects[objects.Length - 1]))
                {
                    builder.NewRecord();
                }
            }

            var statement = builder.ToString();

            if (disableIdentityInsert)
            {
                statement = $"SET IDENTITY_INSERT [{tableName}] ON;" + statement + $"SET IDENTITY_INSERT [{tableName}] OFF;";
            }

            return database.ExecuteNonQuery(statement);
        }

        /// <summary>
        /// Clear all in the database.
        /// </summary>
        /// <param name="database">SQL Server database which the data have to be deleted.</param>
        public static void ClearAllData(this SqlServerDatabase database)
        {
            database.ExecuteNonQuery("EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all'");

            database.ExecuteNonQuery("EXEC sp_msforeachtable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'");

            // Re-initialize the seed of the IDENTITY columns.
            // For each table which contains an IDENTITY column, execute the following SQL statement:
            //   DBCC CHECKIDENT ('[<schema>].[<table>]', RESEED, <seed>)
            database.ExecuteNonQuery(@"
                DECLARE @sqlcmd VARCHAR(MAX);

                SET @sqlcmd = (
	                SELECT STRING_AGG(CAST('DBCC CHECKIDENT (''[' + [s].[name] + '].[' + [t].[name] + ']'', RESEED, ' + CAST([ic].[seed_value] AS VARCHAR(20)) + ')' AS NVARCHAR(MAX)),';' + CHAR(10)) WITHIN GROUP (ORDER BY [t].[name])
	                FROM
		                [sys].[schemas] AS [s],
		                [sys].[tables] AS [t],
		                [sys].[columns] AS [c],
		                [sys].[identity_columns] AS [ic]
	                WHERE
			                [s].[schema_id] = [t].[schema_id]
		                AND [t].[object_id] = [c].[object_id]
		                AND [c].[is_identity] = 1
		                AND [c].[object_id] = [ic].[object_id]
		                AND [c].[column_id] = [ic].[column_id]
                )

                EXEC (@sqlcmd)");

            database.ExecuteNonQuery("EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all'");
        }

        /// <summary>
        /// Execute an T-SQL script on the <paramref name="database"/>.
        /// </summary>
        /// <param name="database"><see cref="SqlServerDatabase"/> where the <paramref name="script"/> will be executed.</param>
        /// <param name="script">T-SQL script to execute.</param>
        public static void ExecuteScript(this SqlServerDatabase database, string script)
        {
            using var stringReader = new StringReader(script);

            ExecuteScript(database, stringReader);
        }

        /// <summary>
        /// Execute an T-SQL script on the <paramref name="database"/>.
        /// </summary>
        /// <param name="database"><see cref="SqlServerDatabase"/> where the <paramref name="script"/> will be executed.</param>
        /// <param name="script"><see cref="StringReader"/> which contains the T-SQL script to execute.</param>
        public static void ExecuteScript(this SqlServerDatabase database, StringReader script)
        {
            var parser = new SqlServerScriptParser(script);

            var block = parser.ReadNextBlock();

            while (block is not null)
            {
                for (var i = 0; i < block.Count; i++)
                {
                    database.ExecuteNonQuery(block.Code);
                }

                block = parser.ReadNextBlock();
            }
        }

        private sealed class SqlInsertStatementBuilder
        {
            private readonly string tableName;
            private readonly List<string> columns;
            private readonly List<List<string>> records;

            private List<string> currentRecord;

            public SqlInsertStatementBuilder(string tableName)
            {
                this.tableName = tableName;
                this.columns = [];
                this.currentRecord = [];
                this.records = [];
            }

            public SqlInsertStatementBuilder NewRecord()
            {
                this.records.Add(this.currentRecord);
                this.currentRecord = [];

                return this;
            }

            public SqlInsertStatementBuilder AddValue(byte[] value)
            {
                var hexValue = BitConverter.ToString(value).Replace("-", string.Empty);

                this.currentRecord.Add($"0x{hexValue}");

                return this;
            }

            public SqlInsertStatementBuilder AddValue(string value)
            {
                this.currentRecord.Add(value);

                return this;
            }

            public SqlInsertStatementBuilder AddValueWithQuotes(string value)
            {
                this.currentRecord.Add($"'{value}'");

                return this;
            }

            public SqlInsertStatementBuilder AddColumn(string column)
            {
                this.columns.Add(column);

                return this;
            }

            public override string ToString()
            {
                this.records.Add(this.currentRecord);

                var builder = new StringBuilder();

                builder.AppendLine();
                builder.AppendLine($"INSERT INTO [{this.tableName}]");
                builder.AppendLine($"{Tab}({string.Join(",    ", this.columns.Select(c => $"[{c}]"))})");
                builder.AppendLine($"VALUES");

                builder.Append($"{Tab}({string.Join(", ", this.records[0])})");

                for (var i = 1; i < this.records.Count; i++)
                {
                    builder.AppendLine(",");
                    builder.Append($"{Tab}({string.Join(", ", this.records[i])})");
                }

                builder.AppendLine();

                return builder.ToString();
            }
        }
    }
}
