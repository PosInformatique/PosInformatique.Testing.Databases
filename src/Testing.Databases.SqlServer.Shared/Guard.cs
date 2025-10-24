//-----------------------------------------------------------------------
// <copyright file="Guard.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique
{
    internal static class Guard
    {
        public static void ThrowIfNull(object? value, string paramName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
