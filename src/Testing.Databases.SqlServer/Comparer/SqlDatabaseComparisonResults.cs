﻿//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseComparisonResults.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the differences between 2 databases.
    /// </summary>
    public class SqlDatabaseComparisonResults
    {
        internal SqlDatabaseComparisonResults(
            IList<SqlObjectDifferences<SqlStoredProcedure>> storedProcedures,
            IList<SqlTableDifferences> tables,
            IList<SqlObjectDifferences<SqlUserType>> userTypes,
            IList<SqlObjectDifferences<SqlView>> views)
        {
            this.StoredProcedures = new ReadOnlyCollection<SqlObjectDifferences<SqlStoredProcedure>>(storedProcedures);
            this.Tables = new ReadOnlyCollection<SqlTableDifferences>(tables);
            this.UserTypes = new ReadOnlyCollection<SqlObjectDifferences<SqlUserType>>(userTypes);
            this.Views = new ReadOnlyCollection<SqlObjectDifferences<SqlView>>(views);
        }

        /// <summary>
        /// Gets the stored procedures which are different between two databases.
        /// </summary>
        public ReadOnlyCollection<SqlObjectDifferences<SqlStoredProcedure>> StoredProcedures { get; }

        /// <summary>
        /// Gets the tables which are different between two databases.
        /// </summary>
        public ReadOnlyCollection<SqlTableDifferences> Tables { get; }

        /// <summary>
        /// Gets the user types which are different between two databases.
        /// </summary>
        public ReadOnlyCollection<SqlObjectDifferences<SqlUserType>> UserTypes { get; }

        /// <summary>
        /// Gets the views which are different between two databases.
        /// </summary>
        public ReadOnlyCollection<SqlObjectDifferences<SqlView>> Views { get; }

        /// <summary>
        /// Gets a value indicating whether if the two database compared have the same schema.
        /// </summary>
        public bool IsIdentical
        {
            get
            {
                if (this.StoredProcedures.Count > 0)
                {
                    return false;
                }

                if (this.Tables.Count > 0)
                {
                    return false;
                }

                if (this.UserTypes.Count > 0)
                {
                    return false;
                }

                if (this.Views.Count > 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Returns a textual representation of the result comparison.
        /// </summary>
        /// <returns>A textual representation of the result comparison.</returns>
        public override string ToString()
        {
            return SqlDatabaseComparisonResultsTextGenerator.Generate(this);
        }
    }
}
