//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseObjectDifferenceType.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    /// <summary>
    /// Indicate the difference type between <see cref="SqlDatabaseObjectDifference.Source"/> and <see cref="SqlDatabaseObjectDifference.Target"/>.
    /// </summary>
    public enum SqlDatabaseObjectDifferenceType
    {
        /// <summary>
        /// Indicates that an object exists in the source database but does not exists in the target database.
        /// </summary>
        MissingInTarget,

        /// <summary>
        /// Indicates that an object exists in the target database but does not exists in the source database.
        /// </summary>
        MissingInSource,

        /// <summary>
        /// Indicates that the object exists in the two databases but have some different properties.
        /// </summary>
        Different,
    }
}
