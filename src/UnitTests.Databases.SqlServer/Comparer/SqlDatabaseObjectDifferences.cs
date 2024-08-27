//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseObjectDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents a list of the difference for database object type.
    /// </summary>
    public sealed class SqlDatabaseObjectDifferences
    {
        internal SqlDatabaseObjectDifferences(string type, IList<SqlDatabaseObjectDifference> differences)
        {
            this.Differences = new ReadOnlyCollection<SqlDatabaseObjectDifference>(differences);
            this.Type = type;
        }

        /// <summary>
        /// Gets the list of the object differences between the two databases.
        /// </summary>
        public ReadOnlyCollection<SqlDatabaseObjectDifference> Differences { get; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        public string Type { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.Type;
        }
    }
}
