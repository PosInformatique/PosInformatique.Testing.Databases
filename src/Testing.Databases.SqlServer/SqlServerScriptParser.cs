//-----------------------------------------------------------------------
// <copyright file="SqlServerScriptParser.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    using System.Globalization;
    using System.Text;

    internal sealed class SqlServerScriptParser
    {
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

                if (line.StartsWith("GO"))
                {
                    // Parse the number after the "GO".
                    var textAfterGo = line.Substring(2).Trim();

                    if (textAfterGo != string.Empty)
                    {
                        count = Convert.ToInt32(textAfterGo, CultureInfo.InvariantCulture);
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

            return new SqlServerScriptBlock(codeBuilder.ToString(), count);
        }
    }
}
