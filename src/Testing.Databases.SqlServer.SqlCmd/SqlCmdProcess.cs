//-----------------------------------------------------------------------
// <copyright file="SqlCmdProcess.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using System.Diagnostics;
    using Microsoft.Data.SqlClient;

    internal sealed class SqlCmdProcess : IDisposable
    {
        private Process? process;

        private List<string> output;

        private SqlCmdProcess(string arguments)
        {
            this.output = new List<string>();

            this.process = new Process()
            {
                StartInfo =
                {
                    Arguments = arguments,
                    FileName = "sqlcmd",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                },
            };

            this.process.ErrorDataReceived += this.OnOutputDataReceived;
            this.process.OutputDataReceived += this.OnOutputDataReceived;

            this.process.Start();

            this.process.BeginErrorReadLine();
            this.process.BeginOutputReadLine();
        }

        public string Output => string.Join(Environment.NewLine, this.output);

        public static SqlCmdProcess RunScript(SqlConnectionStringBuilder connectionString, string inputFile, SqlCmdRunScriptSettings settings)
        {
            var commandLineBuilder = new SqlCmdCommandLineArgumentsBuilder(connectionString)
            {
                InputFile = inputFile,
            };

            foreach (var variable in settings.Variables)
            {
                commandLineBuilder.AddVariable(variable.Key, variable.Value);
            }

            var process = new SqlCmdProcess(commandLineBuilder.ToString());

            return process;
        }

        public void Dispose()
        {
            if (this.process is not null)
            {
                this.process.Dispose();
                this.process = null;
            }
        }

        public int WaitForExit()
        {
            if (this.process is null)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            this.process.WaitForExit();

            return this.process.ExitCode;
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data is not null)
            {
                lock (this.output)
                {
                    this.output.Add(e.Data);
                }
            }
        }
    }
}
