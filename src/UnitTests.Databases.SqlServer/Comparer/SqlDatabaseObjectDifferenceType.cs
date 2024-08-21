//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseObjectDifferenceType.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    public enum SqlDatabaseObjectDifferenceType
    {
        MissingInTarget,

        MissingInSource,

        Different,
    }
}
