//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Globalization;
    using System.Text;
    using Microsoft.Data.SqlClient;

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

        public static int InsertInto<T>(this SqlServerDatabase database, string tableName, params T[] objects)
        {
            return InsertInto(database, tableName, false, objects);
        }

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

                if (!@object!.Equals(objects[^1]))
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

        private sealed class SqlInsertStatementBuilder
        {
            private readonly string tableName;
            private readonly List<string> columns;
            private readonly List<List<string>> records;

            private List<string> currentRecord;

            public SqlInsertStatementBuilder(string tableName)
            {
                this.tableName = tableName;
                this.columns = new();
                this.currentRecord = new();
                this.records = new();
            }

            public SqlInsertStatementBuilder NewRecord()
            {
                this.records.Add(this.currentRecord);
                this.currentRecord = new();

                return this;
            }

            public SqlInsertStatementBuilder AddValue(byte[] value)
            {
                var hexValue = BitConverter.ToString(value).Replace("-", string.Empty, StringComparison.InvariantCultureIgnoreCase);

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
