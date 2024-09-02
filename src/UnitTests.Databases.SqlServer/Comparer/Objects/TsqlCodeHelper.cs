//-----------------------------------------------------------------------
// <copyright file="TsqlCodeHelper.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal static class TsqlCodeHelper
    {
        public static bool AreEqual(string source, string target)
        {
            source = RemoveNotUsefulCharacters(source);
            target = RemoveNotUsefulCharacters(target);

            return source.Equals(target, StringComparison.InvariantCulture);
        }

        private static string RemoveNotUsefulCharacters(string code)
        {
            return code
                .ReplaceLineEndings(string.Empty)
                .Replace(" ", string.Empty)
                .Replace("\t", string.Empty);
        }
    }
}
