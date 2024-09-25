//-----------------------------------------------------------------------
// <copyright file="ISqlDatabaseObjectDifferencesVisitor.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    internal interface ISqlDatabaseObjectDifferencesVisitor
    {
        void Visit<TSqlObject>(SqlDatabaseObjectDifferences<TSqlObject> differences)
            where TSqlObject : SqlObject;

        void Visit(SqlForeignKeyDifferences differences);

        void Visit(SqlIndexDifferences differences);

        void Visit(SqlPrimaryKeyDifferences differences);

        void Visit(SqlDatabaseTableDifferences differences);

        void Visit(SqlUniqueConstraintDifferences differences);
    }
}
