//-----------------------------------------------------------------------
// <copyright file="SqlColumnIdentity.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents the <see cref="SqlColumn.Identity"/> information when
    /// a <see cref="SqlColumn"/> is an <c>IDENTITY</c> column.
    /// </summary>
    public class SqlColumnIdentity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlColumnIdentity"/> class.
        /// </summary>
        /// <param name="seed">Seed of the <c>IDENTITY</c> column.</param>
        /// <param name="increment">Increment of the <c>IDENTITY</c> column.</param>
        public SqlColumnIdentity(int seed, int increment)
        {
            this.Seed = seed;
            this.Increment = increment;
        }

        /// <summary>
        /// Gets the seed of the <c>IDENTITY</c> column.
        /// </summary>
        public int Seed { get; }

        /// <summary>
        /// Gets the increment of the <c>IDENTITY</c> column.
        /// </summary>
        public int Increment { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"(Seed: {this.Seed}, Increment: {this.Increment})";
        }
    }
}
