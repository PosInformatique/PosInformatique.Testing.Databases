//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseObjectDifference.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Data;
    using System.Text;

    /// <summary>
    /// Represents a difference between 2 database objects.
    /// </summary>
    public class SqlDatabaseObjectDifference
    {
        internal SqlDatabaseObjectDifference(DataRow? source, DataRow? target, SqlDatabaseObjectDifferenceType type, object[] keyValue)
        {
            this.Source = DataRowToDictionary(source);
            this.Target = DataRowToDictionary(target);
            this.Type = type;
            this.Name = string.Join(".", keyValue);
        }

        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets schema information of the source object. If <see langword="null"/> it is mean
        /// that the object exists in the target database but does not exists in the source database.
        /// </summary>
        public IReadOnlyDictionary<string, object>? Source { get; }

        /// <summary>
        /// Gets schema information of the target object. If <see langword="null"/> it is mean
        /// that the object exists in the source database but does not exists in the target database.
        /// </summary>
        public IReadOnlyDictionary<string, object>? Target { get; }

        /// <summary>
        /// Gets the of the difference between the <see cref="Source"/> and the <see cref="Target"/>.
        /// </summary>
        public SqlDatabaseObjectDifferenceType Type { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            if (this.Type == SqlDatabaseObjectDifferenceType.MissingInSource)
            {
                return $"(Missing) <=> {this.Name}";
            }
            else if (this.Type == SqlDatabaseObjectDifferenceType.MissingInTarget)
            {
                return $"{this.Name} <=> (Missing)";
            }
            else
            {
                var stringBuilder = new StringBuilder(this.Name);
                stringBuilder.Append(':');

                foreach (var key in this.Source!.Keys)
                {
                    var sourceValue = this.Source[key];
                    var targetValue = this.Target![key];

                    if (!Equals(sourceValue, targetValue))
                    {
                        stringBuilder.AppendLine();
                        stringBuilder.Append("- ");
                        stringBuilder.Append(key);
                        stringBuilder.Append(": (Source: ");
                        stringBuilder.Append(sourceValue);
                        stringBuilder.Append(", Target: ");
                        stringBuilder.Append(targetValue);
                        stringBuilder.Append(')');
                    }
                }

                return stringBuilder.ToString();
            }
        }

        private static Dictionary<string, object>? DataRowToDictionary(DataRow? dataRow)
        {
            if (dataRow is null)
            {
                return null;
            }

            return dataRow.Table.Columns.Cast<DataColumn>()
                .ToDictionary(
                    c => c.ColumnName,
                    c => dataRow[c]);
        }
    }
}
