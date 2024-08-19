//-----------------------------------------------------------------------
// <copyright file="SqlServerExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using Microsoft.SqlServer.Dac;

    public static class SqlServerExtensions
    {
        public static SqlServerDatabase DeployDacPackage(this SqlServer server, string fileName, string databaseName)
        {
            using (var package = DacPackage.Load(fileName))
            {
                var options = new DacDeployOptions();
                options.CreateNewDatabase = true;
                options.ExcludeObjectTypes = [ObjectType.Logins];

                var services = new DacServices(server.ConnectionString);
                services.Deploy(package, databaseName, true, options: options);
            }

            return server.GetDatabase(databaseName);
        }
    }
}
