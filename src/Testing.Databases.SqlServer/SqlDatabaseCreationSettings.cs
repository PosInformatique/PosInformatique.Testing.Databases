//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseCreationSettings.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    /// <summary>
    /// Settings of the <see cref="SqlServerDatabase"/> to create using the
    /// <see cref="SqlServer.CreateEmptyDatabase(string, SqlDatabaseCreationSettings?)"/>
    /// or
    /// <see cref="SqlServer.CreateEmptyDatabaseAsync(string, SqlDatabaseCreationSettings?, CancellationToken)"/>
    /// methods.
    /// </summary>
    public class SqlDatabaseCreationSettings
    {
        /// <summary>
        /// Gets or sets the data file name (full path) of the database to create.
        /// </summary>
        public string? DataFileName { get; set; }
    }
}
