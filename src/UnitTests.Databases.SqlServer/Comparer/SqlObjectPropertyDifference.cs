//-----------------------------------------------------------------------
// <copyright file="SqlObjectPropertyDifference.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    /// <summary>
    /// Represents a difference of property value of compared <see cref="SqlObject"/>.
    /// </summary>
    public class SqlObjectPropertyDifference
    {
        internal SqlObjectPropertyDifference(string name, object? source, object? target)
        {
            this.Name = name;
            this.Source = source;
            this.Target = target;
        }

        /// <summary>
        /// Gets the name of the property compared.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the source value of the property.
        /// </summary>
        public object? Source { get; }

        /// <summary>
        /// Gets the target value of the property.
        /// </summary>
        public object? Target { get; }
    }
}
