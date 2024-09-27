//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseComparerOptions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Options for the comparison between two SQL database.
    /// </summary>
    public class SqlDatabaseComparerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabaseComparerOptions"/> class.
        /// </summary>
        public SqlDatabaseComparerOptions()
        {
            this.ExcludedTables = [];
        }

        /// <summary>
        /// Gets the list of the excluded SQL table during the comparison.
        /// </summary>
        public Collection<string> ExcludedTables { get; }
    }
}
