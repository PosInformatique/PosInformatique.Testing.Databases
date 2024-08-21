//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseComparisonResults.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Collections.ObjectModel;
    using System.Text;

    public class SqlDatabaseComparisonResults
    {
        internal SqlDatabaseComparisonResults(IList<SqlDatabaseObjectDifferences> objects)
        {
            this.Objects = new ReadOnlyCollection<SqlDatabaseObjectDifferences>(objects);
        }

        public ReadOnlyCollection<SqlDatabaseObjectDifferences> Objects { get; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var @object in this.Objects)
            {
                stringBuilder.AppendLine("------------------------");
                stringBuilder.AppendLine(@object.Type);

                foreach (var difference in @object.Differences)
                {
                    stringBuilder.AppendLine(difference.ToString());
                }

                stringBuilder.AppendLine("------------------------");
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
