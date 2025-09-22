//-----------------------------------------------------------------------
// <copyright file="SqlServerDacDeploymentSettings.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    /// <summary>
    /// Contains additional settings when deploying a <see cref="SqlServerDatabase"/>
    /// using <see cref="SqlServerDacExtensions"/> or <see cref="SqlServerDacDatabaseInitializer"/>.
    /// </summary>
    public class SqlServerDacDeploymentSettings
    {
        /// <summary>
        /// Gets or sets the data file name (full path) of the database to create.
        /// </summary>
        public string? DataFileName { get; set; }
    }
}
