//-----------------------------------------------------------------------
// <copyright file="SqlUniqueConstraintDifferences.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences of a <see cref="SqlUniqueConstraintDifferences"/> between two databases.
    /// </summary>
    public class SqlUniqueConstraintDifferences : SqlDatabaseObjectDifferences<SqlUniqueConstraint>
    {
        internal SqlUniqueConstraintDifferences(
            SqlUniqueConstraint? source,
            SqlUniqueConstraint? target,
            SqlObjectDifferenceType type,
            IReadOnlyList<SqlObjectPropertyDifference>? properties,
            IList<SqlDatabaseObjectDifferences<SqlIndexColumn>> columns)
            : base(source, target, type, properties)
        {
            this.Columns = new ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlIndexColumn>>(columns);
        }

        internal SqlUniqueConstraintDifferences(
            SqlDatabaseObjectDifferences<SqlUniqueConstraint> differences)
            : base(differences.Source, differences.Target, differences.Type, differences.Properties)
        {
        }

        /// <summary>
        /// Gets the difference of the columns in the unique constraint compared.
        /// </summary>
        public ReadOnlyCollection<SqlDatabaseObjectDifferences<SqlIndexColumn>> Columns { get; }
    }
}
