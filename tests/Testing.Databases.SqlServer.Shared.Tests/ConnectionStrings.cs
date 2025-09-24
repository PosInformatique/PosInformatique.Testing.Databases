//-----------------------------------------------------------------------
// <copyright file="ConnectionStrings.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using Microsoft.Data.SqlClient;

    public static class ConnectionStrings
    {
        public static string Get(string databaseName = "master")
        {
            var connectionString = Environment.GetEnvironmentVariable("SQL_SERVER_UNIT_TESTS_CONNECTION_STRING");

            if (connectionString is null)
            {
                connectionString = $"Data Source=(localDB)\\posinfo-tests; Integrated Security=True";
            }

            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = databaseName,
            };

            return connectionStringBuilder.ToString();
        }
    }
}
