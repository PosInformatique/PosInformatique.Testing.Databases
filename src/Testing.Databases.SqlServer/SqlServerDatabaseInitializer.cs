//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseInitializer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    /// <summary>
    /// Initializer used to initialize the database for the unit tests.
    /// Depending of the strategy to use (initialize from Entity Framework DbContext or .dacpac package)
    /// add the <c>PosInformatique.UnitTests.Databases.SqlServer.EntityFramework</c> or <c>PosInformatique.UnitTests.Databases.SqlServer.Dac</c>
    /// NuGet packages and call the <c>Initialize()</c> method.
    /// </summary>
    public class SqlServerDatabaseInitializer
    {
        /// <summary>
        /// Gets or sets a value indicating whether if the database has been initialized.
        /// </summary>
        public bool IsInitialized { get; set; }
    }
}
