//-----------------------------------------------------------------------
// <copyright file="SqlServerDacExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using Microsoft.SqlServer.Dac;

    /// <summary>
    /// Extensions methods of the <see cref="SqlServer"/> class.
    /// </summary>
    public static class SqlServerDacExtensions
    {
        /// <summary>
        /// Deploy a database using a DACPAC file.
        /// </summary>
        /// <remarks>If a database with the <paramref name="databaseName"/> already exists, it will be deleted.</remarks>
        /// <param name="server"><see cref="SqlServer"/> instance where the DACPAC file will be deployed.</param>
        /// <param name="fileName">File name (including the path) for the DACPAC file to deploy.</param>
        /// <param name="databaseName">Name of the database which will be created.</param>
        /// <returns>An instance of the <see cref="SqlServerDatabase"/> which represents the deployed database.</returns>
        public static SqlServerDatabase DeployDacPackage(this SqlServer server, string fileName, string databaseName)
        {
            using (var package = DacPackage.Load(fileName))
            {
                var options = new DacDeployOptions();
                options.CreateNewDatabase = true;

                var services = new DacServices(server.Master.ConnectionString);
                services.Deploy(package, databaseName, true, options: options);
            }

            return server.GetDatabase(databaseName);
        }
    }
}
