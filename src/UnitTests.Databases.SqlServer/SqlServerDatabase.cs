//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabase.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    using System.Data;
    using Microsoft.Data.SqlClient;

    public sealed class SqlServerDatabase
    {
        internal SqlServerDatabase(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public int ExecuteNonQuery(string command)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.ConnectionString = this.ConnectionString;
                connection.Open();

                using (var dbCommand = connection.CreateCommand())
                {
                    dbCommand.CommandText = command;

                    return dbCommand.ExecuteNonQuery();
                }
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            using (var adapter = new SqlDataAdapter(query, this.ConnectionString))
            {
                var dataTable = new DataTable();

                adapter.Fill(dataTable);

                return dataTable;
            }
        }
    }
}
