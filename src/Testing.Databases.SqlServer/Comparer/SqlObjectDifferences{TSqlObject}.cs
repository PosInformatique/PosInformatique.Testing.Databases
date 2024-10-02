//-----------------------------------------------------------------------
// <copyright file="SqlObjectDifferences{TSqlObject}.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences of <typeparamref name="TSqlObject"/> between two databases.
    /// </summary>
    /// <typeparam name="TSqlObject">Type of the <see cref="SqlObject"/> compared.</typeparam>
    public class SqlObjectDifferences<TSqlObject> : SqlObjectDifferences
        where TSqlObject : SqlObject
    {
        internal SqlObjectDifferences(TSqlObject? source, TSqlObject? target, SqlObjectDifferenceType type, IReadOnlyList<SqlObjectPropertyDifference>? properties)
        {
            this.Source = source;
            this.Target = target;
            this.Type = type;

            if (properties is not null)
            {
                this.Properties = new ReadOnlyCollection<SqlObjectPropertyDifference>(properties.ToArray());
            }
            else
            {
                this.Properties = new ReadOnlyCollection<SqlObjectPropertyDifference>([]);
            }
        }

        /// <summary>
        /// Gets the source <see cref="SqlObject"/> compared.
        /// </summary>
        public TSqlObject? Source { get; }

        /// <summary>
        /// Gets the target <see cref="SqlObject"/> compared.
        /// </summary>
        public TSqlObject? Target { get; }

        /// <summary>
        /// Gets the of the difference between the <see cref="Source"/> and the <see cref="Target"/>.
        /// </summary>
        public SqlObjectDifferenceType Type { get; }

        /// <summary>
        /// Gets the property changes between <see cref="Source"/> and <see cref="Target"/>.
        /// </summary>
        public ReadOnlyCollection<SqlObjectPropertyDifference> Properties { get; }

        /// <summary>
        /// Returns a textual representation of the result comparison.
        /// </summary>
        /// <returns>A textual representation of the result comparison.</returns>
        public override string ToString()
        {
            return SqlDatabaseComparisonResultsTextGenerator.Generate(this);
        }

        internal virtual void Accept(ISqlObjectDifferencesVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
