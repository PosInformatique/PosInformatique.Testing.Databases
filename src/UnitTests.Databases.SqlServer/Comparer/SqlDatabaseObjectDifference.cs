//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseObjectDifference.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Data;
    using System.Text;

    public class SqlDatabaseObjectDifference
    {
        private readonly object[] keyValue;

        internal SqlDatabaseObjectDifference(DataRow? source, DataRow? target, SqlDatabaseObjectDifferenceType type, object[] keyValue)
        {
            this.Source = source;
            this.Target = target;
            this.Type = type;
            this.keyValue = keyValue;
        }

        public DataRow? Source { get; }

        public DataRow? Target { get; }

        public SqlDatabaseObjectDifferenceType Type { get; }

        public override string ToString()
        {
            var keyValueString = string.Join(".", this.keyValue);

            if (this.Type == SqlDatabaseObjectDifferenceType.MissingInSource)
            {
                return $"(Missing) <=> {keyValueString}";
            }
            else if (this.Type == SqlDatabaseObjectDifferenceType.MissingInTarget)
            {
                return $"{keyValueString} <=> (Missing)";
            }
            else
            {
                var stringBuilder = new StringBuilder(keyValueString);
                stringBuilder.Append(":");

                foreach (var sourceColumn in this.Source!.Table.Columns.Cast<DataColumn>())
                {
                    var sourceValue = this.Source[sourceColumn];
                    var targetValue = this.Target![sourceColumn.ColumnName];

                    if (!Equals(sourceValue, targetValue))
                    {
                        stringBuilder.AppendLine();
                        stringBuilder.Append("- ");
                        stringBuilder.Append(sourceColumn.ColumnName);
                        stringBuilder.Append(": (Source: ");
                        stringBuilder.Append(sourceValue);
                        stringBuilder.Append(", Target: ");
                        stringBuilder.Append(targetValue);
                        stringBuilder.Append(")");
                    }
                }

                return stringBuilder.ToString();
            }
        }
    }
}
