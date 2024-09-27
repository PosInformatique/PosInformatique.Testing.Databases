//-----------------------------------------------------------------------
// <copyright file="TsqlCodeHelper.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    internal static class TsqlCodeHelper
    {
        public static string? RemoveNotUsefulCharacters(string? code)
        {
            if (code is null)
            {
                return null;
            }

            return code
                .ReplaceLineEndings(string.Empty)
                .Replace(" ", string.Empty)
                .Replace("\t", string.Empty);
        }
    }
}
