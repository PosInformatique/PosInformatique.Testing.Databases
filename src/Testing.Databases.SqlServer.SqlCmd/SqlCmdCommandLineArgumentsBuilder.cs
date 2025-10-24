//-----------------------------------------------------------------------
// <copyright file="SqlCmdCommandLineArgumentsBuilder.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using System.Text;
    using Microsoft.Data.SqlClient;

    internal sealed class SqlCmdCommandLineArgumentsBuilder
    {
        private readonly Dictionary<string, string> variables;

        public SqlCmdCommandLineArgumentsBuilder(SqlConnectionStringBuilder connectionString)
        {
            this.Database = connectionString.InitialCatalog;
            this.LoginId = connectionString.UserID;
            this.Password = connectionString.Password;
            this.Server = connectionString.DataSource;
            this.TrustedConnection = connectionString.IntegratedSecurity;

            this.variables = new Dictionary<string, string>();
        }

        public string? Database { get; set; }

        public string? InputFile { get; set; }

        public string? LoginId { get; set; }

        public string? Password { get; set; }

        public string? Server { get; set; }

        public bool TrustedConnection { get; set; }

        public IReadOnlyDictionary<string, string> Variables => this.variables;

        public void AddVariable(string name, string value)
        {
            this.variables.Add(name, value);
        }

        public override string ToString()
        {
            var parameters = new StringBuilder();

            if (!string.IsNullOrEmpty(this.Database))
            {
                parameters.Append($"-d \"{this.Database}\" ");
            }

            if (!string.IsNullOrEmpty(this.InputFile))
            {
                parameters.Append($"-i \"{this.InputFile}\" ");
            }

            if (!string.IsNullOrEmpty(this.LoginId))
            {
                parameters.Append($"-U \"{this.LoginId}\" ");
            }

            if (!string.IsNullOrEmpty(this.Password))
            {
                parameters.Append($"-P \"{this.Password}\" ");
            }

            if (this.TrustedConnection)
            {
                parameters.Append($"-E ");
            }

            if (!string.IsNullOrEmpty(this.Server))
            {
                parameters.Append($"-S \"{this.Server}\" ");
            }

            foreach (var variable in this.variables)
            {
                parameters.Append($"-v {variable.Key}=\"{variable.Value}\" ");
            }

            // To have exit error code when the script contains errors.
            parameters.Append("-b");

            return parameters.ToString();
        }
    }
}
