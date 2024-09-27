//-----------------------------------------------------------------------
// <copyright file="ISqlObjectDifferencesVisitor.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    internal interface ISqlObjectDifferencesVisitor
    {
        void Visit<TSqlObject>(SqlObjectDifferences<TSqlObject> differences)
            where TSqlObject : SqlObject;

        void Visit(SqlForeignKeyDifferences differences);

        void Visit(SqlIndexDifferences differences);

        void Visit(SqlPrimaryKeyDifferences differences);

        void Visit(SqlTableDifferences differences);

        void Visit(SqlUniqueConstraintDifferences differences);
    }
}
