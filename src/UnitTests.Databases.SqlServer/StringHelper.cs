//-----------------------------------------------------------------------
// <copyright file="StringHelper.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    internal static class StringHelper
    {
        public static string ToStringNull(object? @object, string valueIfNull = "null")
        {
            if (@object is null)
            {
                return valueIfNull;
            }

            return @object.ToString()!;
        }
    }
}
