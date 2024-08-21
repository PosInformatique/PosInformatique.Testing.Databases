//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseObjectDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Collections.ObjectModel;

    public sealed class SqlDatabaseObjectDifferences
    {
        internal SqlDatabaseObjectDifferences(string type, IList<SqlDatabaseObjectDifference> differences)
        {
            this.Differences = new ReadOnlyCollection<SqlDatabaseObjectDifference>(differences);
            this.Type = type;
        }

        public ReadOnlyCollection<SqlDatabaseObjectDifference> Differences { get; }

        public string Type { get; }

        public override string ToString()
        {
            return this.Type;
        }
    }
}
