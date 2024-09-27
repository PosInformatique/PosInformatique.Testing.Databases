//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabase.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using System.Data;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Represents a SQL Server database.
    /// </summary>
    public sealed class SqlServerDatabase
    {
        internal SqlServerDatabase(SqlServer server, string connectionString)
        {
            this.ConnectionString = connectionString;
            this.Server = server;
        }

        /// <summary>
        /// Gets the connection string to the SQL Server database.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Gets a reference to the <see cref="SqlServer"/> which host the database.
        /// </summary>
        public SqlServer Server { get; }

        /// <summary>
        /// Executes a command (non-SELECT query) on the database.
        /// </summary>
        /// <param name="command">SQL command to execute (INSERT, UPDATE, DELETE, DROP,...)</param>
        /// <returns>The number of "rows" impacted by the command.</returns>
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

        /// <summary>
        /// Executes a command (non-SELECT query) on the database asynchronously.
        /// </summary>
        /// <param name="command">SQL command to execute (INSERT, UPDATE, DELETE, DROP,...)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> used to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="Task"/> which represents the asynchronous operation and contains the number of "rows" impacted by the command.</returns>
        public async Task<int> ExecuteNonQueryAsync(string command, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                connection.ConnectionString = this.ConnectionString;
                await connection.OpenAsync(cancellationToken);

                using (var dbCommand = connection.CreateCommand())
                {
                    dbCommand.CommandText = command;

                    return await dbCommand.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Executes a SQL SELECT query on the database and returns the data in a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="query">SELECT query to execute.</param>
        /// <returns>A <see cref="DataTable"/> which contains the results of the SQL query by rows / columns.</returns>
        public DataTable ExecuteQuery(string query)
        {
            using (var adapter = new SqlDataAdapter(query, this.ConnectionString))
            {
                var dataTable = new DataTable();

                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        /// <summary>
        /// Executes a SQL SELECT query asynchronously on the database and returns the data in a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="query">SELECT query to execute.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> used to cancel the asynchronous operation.</param>
        /// <returns>A task which represents the asynchronous operation that contains the <see cref="DataTable"/> which contains the results of the SQL query by rows / columns.</returns>
        public async Task<DataTable> ExecuteQueryAsync(string query, CancellationToken cancellationToken = default)
        {
            var dataTable = new DataTable();

            using (var connection = new SqlConnection(this.ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;

                await connection.OpenAsync(cancellationToken);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    dataTable.Load(reader);
                }
            }

            return dataTable;
        }
    }
}
