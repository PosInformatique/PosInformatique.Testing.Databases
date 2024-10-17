//-----------------------------------------------------------------------
// <copyright file="SqlServerScriptBlock.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    internal class SqlServerScriptBlock
    {
        public SqlServerScriptBlock(string code, int count)
        {
            this.Code = code;
            this.Count = count;
        }

        public string Code { get; }

        public int Count { get; }
    }
}
