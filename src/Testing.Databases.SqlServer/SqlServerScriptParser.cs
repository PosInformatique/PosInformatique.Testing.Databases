//-----------------------------------------------------------------------
// <copyright file="SqlServerScriptParser.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    internal sealed class SqlServerScriptParser
    {
        private static readonly Regex GoInstruction = new Regex("^GO\\s*(?<count>\\d+)?\\b", RegexOptions.Compiled);

        private readonly TextReader script;

        private bool isEndOfScript;

        public SqlServerScriptParser(TextReader script)
        {
            this.script = script;
        }

        public SqlServerScriptBlock? ReadNextBlock()
        {
            if (this.isEndOfScript)
            {
                return null;
            }

            var codeBuilder = new StringBuilder();

            var count = 1;

            while (true)
            {
                var line = this.script.ReadLine();

                if (line is null)
                {
                    // End of the script reach.
                    this.isEndOfScript = true;
                    break;
                }

                line = line.Trim();

                var goInstructionMatch = GoInstruction.Match(line);

                if (goInstructionMatch.Success)
                {
                    // Retrieve the number after the "GO".
                    if (goInstructionMatch.Groups["count"].Success)
                    {
                        count = Convert.ToInt32(goInstructionMatch.Groups["count"].Value, CultureInfo.InvariantCulture);
                    }

                    // If no code parsed, we continue to parse the block.
                    if (codeBuilder.Length == 0)
                    {
                        continue;
                    }

                    // Else, we stop the read of the script.
                    break;
                }

                codeBuilder.AppendLine(line);
            }

            if (codeBuilder.Length == 0)
            {
                return null;
            }

            return new SqlServerScriptBlock(codeBuilder.ToString(), count);
        }
    }
}
