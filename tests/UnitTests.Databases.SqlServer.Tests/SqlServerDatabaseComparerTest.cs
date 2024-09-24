//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseComparerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer.Tests
{
    [Collection("PosInformatique.UnitTests.Databases.SqlServer.Tests")]
    public class SqlServerDatabaseComparerTest
    {
        private const string ConnectionString = $"Data Source=(localDB)\\posinfo-unit-tests; Integrated Security=True";

        [Fact]
        public void Compare()
        {
            var server = new SqlServer(ConnectionString);

            var sourceDatabase = server.DeployDacPackage("UnitTests.Databases.SqlServer.Tests.Source.dacpac", $"{nameof(SqlServerDatabaseComparerTest)}_Source");
            var targetDatabase = server.DeployDacPackage("UnitTests.Databases.SqlServer.Tests.Target.dacpac", $"{nameof(SqlServerDatabaseComparerTest)}_Target");

            var comparer = new SqlServerDatabaseComparer();

            var differences = comparer.Compare(sourceDatabase, targetDatabase);

            // StoredProcedures
            differences.StoredProcedures.Should().HaveCount(3);

            differences.StoredProcedures[0].Source.Name.Should().Be("StoredProcedureDifference");
            differences.StoredProcedures[0].Target.Name.Should().Be("StoredProcedureDifference");
            differences.StoredProcedures[0].Target.Schema.Should().Be("dbo");
            differences.StoredProcedures[0].Properties.Should().HaveCount(1);
            differences.StoredProcedures[0].Properties[0].Name.Should().Be("Code");
            differences.StoredProcedures[0].Properties[0].Source.Should().Be("CREATE PROCEDURE [dbo].[StoredProcedureDifference]\r\n\t@param1 int = 0,\r\n\t@param2 int\r\nAS\r\n\tSELECT @param2\r\nRETURN 0");
            differences.StoredProcedures[0].Properties[0].Target.Should().Be("CREATE PROCEDURE [dbo].[StoredProcedureDifference]\r\n\t@param1 int = 0,\r\n\t@param2 int\r\nAS\r\n\tSELECT @param1\r\nRETURN 0");

            differences.StoredProcedures[1].Source.Should().BeNull();
            differences.StoredProcedures[1].Target.Name.Should().Be("StoredProcedureTarget");
            differences.StoredProcedures[1].Target.Schema.Should().Be("dbo");

            differences.StoredProcedures[2].Source.Name.Should().Be("StoredProcedureSource");
            differences.StoredProcedures[2].Source.Schema.Should().Be("dbo");
            differences.StoredProcedures[2].Target.Should().BeNull();

            // Tables
            differences.Tables.Should().HaveCount(3);

            differences.Tables[0].Source.Name.Should().Be("TableDifference");

            // Tables / Check constraints
            differences.Tables[0].Source.CheckConstraints.Should().HaveCount(1);

            differences.Tables[0].Source.CheckConstraints[0].Name.Should().Be("CheckConstraintDifference");
            differences.Tables[0].Source.CheckConstraints[0].Code.Should().Be("([Type]>(0))");

            differences.Tables[0].Target.CheckConstraints.Should().HaveCount(1);

            differences.Tables[0].Target.CheckConstraints[0].Name.Should().Be("CheckConstraintDifference");
            differences.Tables[0].Target.CheckConstraints[0].Code.Should().Be("([Type]=(2) OR [Type]=(1))");

            differences.Tables[0].CheckConstraints.Should().HaveCount(1);
            differences.Tables[0].CheckConstraints[0].Properties.Should().HaveCount(1);
            differences.Tables[0].CheckConstraints[0].Properties[0].Name.Should().Be("Code");
            differences.Tables[0].CheckConstraints[0].Properties[0].Source.Should().Be("([Type]>(0))");
            differences.Tables[0].CheckConstraints[0].Properties[0].Target.Should().Be("([Type]=(2) OR [Type]=(1))");
            differences.Tables[0].CheckConstraints[0].Source.Should().BeSameAs(differences.Tables[0].Source.CheckConstraints[0]);
            differences.Tables[0].CheckConstraints[0].Target.Should().BeSameAs(differences.Tables[0].Target.CheckConstraints[0]);
            differences.Tables[0].CheckConstraints[0].Type.Should().Be(SqlObjectDifferenceType.Different);

            // Tables / Columns
            differences.Tables[0].Source.Columns.Should().HaveCount(7);

            differences.Tables[0].Source.Columns[0].CollationName.Should().BeNull();
            differences.Tables[0].Source.Columns[0].ComputedExpression.Should().BeNull();
            differences.Tables[0].Source.Columns[0].IsComputed.Should().BeFalse();
            differences.Tables[0].Source.Columns[0].IsIdentity.Should().BeFalse();
            differences.Tables[0].Source.Columns[0].IsNullable.Should().BeFalse();
            differences.Tables[0].Source.Columns[0].MaxLength.Should().Be(4);
            differences.Tables[0].Source.Columns[0].Name.Should().Be("Type");
            differences.Tables[0].Source.Columns[0].Position.Should().Be(1);
            differences.Tables[0].Source.Columns[0].Precision.Should().Be(10);
            differences.Tables[0].Source.Columns[0].Scale.Should().Be(0);
            differences.Tables[0].Source.Columns[0].SystemTypeId.Should().Be(56);
            differences.Tables[0].Source.Columns[0].TypeName.Should().Be("int");

            differences.Tables[0].Source.Columns[1].CollationName.Should().Be("SQL_Latin1_General_CP1_CI_AS");
            differences.Tables[0].Source.Columns[1].ComputedExpression.Should().BeNull();
            differences.Tables[0].Source.Columns[1].IsComputed.Should().BeFalse();
            differences.Tables[0].Source.Columns[1].IsIdentity.Should().BeFalse();
            differences.Tables[0].Source.Columns[1].IsNullable.Should().BeTrue();
            differences.Tables[0].Source.Columns[1].MaxLength.Should().Be(50);
            differences.Tables[0].Source.Columns[1].Name.Should().Be("Nullable");
            differences.Tables[0].Source.Columns[1].Position.Should().Be(2);
            differences.Tables[0].Source.Columns[1].Precision.Should().Be(0);
            differences.Tables[0].Source.Columns[1].Scale.Should().Be(0);
            differences.Tables[0].Source.Columns[1].SystemTypeId.Should().Be(167);
            differences.Tables[0].Source.Columns[1].TypeName.Should().Be("varchar");

            differences.Tables[0].Source.Columns[2].CollationName.Should().Be("SQL_Latin1_General_CP1_CI_AS");
            differences.Tables[0].Source.Columns[2].ComputedExpression.Should().BeNull();
            differences.Tables[0].Source.Columns[2].IsComputed.Should().BeFalse();
            differences.Tables[0].Source.Columns[2].IsIdentity.Should().BeFalse();
            differences.Tables[0].Source.Columns[2].IsNullable.Should().BeFalse();
            differences.Tables[0].Source.Columns[2].MaxLength.Should().Be(50);
            differences.Tables[0].Source.Columns[2].Name.Should().Be("MaxLength");
            differences.Tables[0].Source.Columns[2].Position.Should().Be(3);
            differences.Tables[0].Source.Columns[2].Precision.Should().Be(0);
            differences.Tables[0].Source.Columns[2].Scale.Should().Be(0);
            differences.Tables[0].Source.Columns[2].SystemTypeId.Should().Be(167);
            differences.Tables[0].Source.Columns[2].TypeName.Should().Be("varchar");

            differences.Tables[0].Source.Columns[3].CollationName.Should().BeNull();
            differences.Tables[0].Source.Columns[3].ComputedExpression.Should().BeNull();
            differences.Tables[0].Source.Columns[3].IsComputed.Should().BeFalse();
            differences.Tables[0].Source.Columns[3].IsIdentity.Should().BeFalse();
            differences.Tables[0].Source.Columns[3].IsNullable.Should().BeFalse();
            differences.Tables[0].Source.Columns[3].MaxLength.Should().Be(9);
            differences.Tables[0].Source.Columns[3].Name.Should().Be("Precision");
            differences.Tables[0].Source.Columns[3].Position.Should().Be(4);
            differences.Tables[0].Source.Columns[3].Precision.Should().Be(10);
            differences.Tables[0].Source.Columns[3].Scale.Should().Be(2);
            differences.Tables[0].Source.Columns[3].SystemTypeId.Should().Be(106);
            differences.Tables[0].Source.Columns[3].TypeName.Should().Be("decimal");

            differences.Tables[0].Source.Columns[4].CollationName.Should().BeNull();
            differences.Tables[0].Source.Columns[4].ComputedExpression.Should().BeNull();
            differences.Tables[0].Source.Columns[4].IsComputed.Should().BeFalse();
            differences.Tables[0].Source.Columns[4].IsIdentity.Should().BeFalse();
            differences.Tables[0].Source.Columns[4].IsNullable.Should().BeFalse();
            differences.Tables[0].Source.Columns[4].MaxLength.Should().Be(9);
            differences.Tables[0].Source.Columns[4].Name.Should().Be("Scale");
            differences.Tables[0].Source.Columns[4].Position.Should().Be(5);
            differences.Tables[0].Source.Columns[4].Precision.Should().Be(10);
            differences.Tables[0].Source.Columns[4].Scale.Should().Be(2);
            differences.Tables[0].Source.Columns[4].SystemTypeId.Should().Be(106);
            differences.Tables[0].Source.Columns[4].TypeName.Should().Be("decimal");

            differences.Tables[0].Source.Columns[5].CollationName.Should().BeNull();
            differences.Tables[0].Source.Columns[5].ComputedExpression.Should().BeNull();
            differences.Tables[0].Source.Columns[5].IsComputed.Should().BeFalse();
            differences.Tables[0].Source.Columns[5].IsIdentity.Should().BeTrue();
            differences.Tables[0].Source.Columns[5].IsNullable.Should().BeFalse();
            differences.Tables[0].Source.Columns[5].MaxLength.Should().Be(4);
            differences.Tables[0].Source.Columns[5].Name.Should().Be("Identity");
            differences.Tables[0].Source.Columns[5].Position.Should().Be(6);
            differences.Tables[0].Source.Columns[5].Precision.Should().Be(10);
            differences.Tables[0].Source.Columns[5].Scale.Should().Be(0);
            differences.Tables[0].Source.Columns[5].SystemTypeId.Should().Be(56);
            differences.Tables[0].Source.Columns[5].TypeName.Should().Be("int");

            differences.Tables[0].Source.Columns[6].CollationName.Should().BeNull();
            differences.Tables[0].Source.Columns[6].ComputedExpression.Should().BeNull();
            differences.Tables[0].Source.Columns[6].IsComputed.Should().BeFalse();
            differences.Tables[0].Source.Columns[6].IsIdentity.Should().BeFalse();
            differences.Tables[0].Source.Columns[6].IsNullable.Should().BeTrue();
            differences.Tables[0].Source.Columns[6].MaxLength.Should().Be(4);
            differences.Tables[0].Source.Columns[6].Name.Should().Be("ForeignKeyId");
            differences.Tables[0].Source.Columns[6].Position.Should().Be(7);
            differences.Tables[0].Source.Columns[6].Precision.Should().Be(10);
            differences.Tables[0].Source.Columns[6].Scale.Should().Be(0);
            differences.Tables[0].Source.Columns[6].SystemTypeId.Should().Be(56);
            differences.Tables[0].Source.Columns[6].TypeName.Should().Be("int");

            differences.Tables[0].Target.Name.Should().Be("TableDifference");
            differences.Tables[0].Target.Schema.Should().Be("dbo");

            differences.Tables[0].Target.Columns.Should().HaveCount(7);

            differences.Tables[0].Target.Columns[0].CollationName.Should().Be("SQL_Latin1_General_CP1_CI_AS");
            differences.Tables[0].Target.Columns[0].ComputedExpression.Should().BeNull();
            differences.Tables[0].Target.Columns[0].IsComputed.Should().BeFalse();
            differences.Tables[0].Target.Columns[0].IsIdentity.Should().BeFalse();
            differences.Tables[0].Target.Columns[0].IsNullable.Should().BeFalse();
            differences.Tables[0].Target.Columns[0].MaxLength.Should().Be(50);
            differences.Tables[0].Target.Columns[0].Name.Should().Be("Type");
            differences.Tables[0].Target.Columns[0].Position.Should().Be(1);
            differences.Tables[0].Target.Columns[0].Precision.Should().Be(0);
            differences.Tables[0].Target.Columns[0].Scale.Should().Be(0);
            differences.Tables[0].Target.Columns[0].SystemTypeId.Should().Be(167);
            differences.Tables[0].Target.Columns[0].TypeName.Should().Be("varchar");

            differences.Tables[0].Target.Columns[1].CollationName.Should().Be("SQL_Latin1_General_CP1_CI_AS");
            differences.Tables[0].Target.Columns[1].ComputedExpression.Should().BeNull();
            differences.Tables[0].Target.Columns[1].IsComputed.Should().BeFalse();
            differences.Tables[0].Target.Columns[1].IsIdentity.Should().BeFalse();
            differences.Tables[0].Target.Columns[1].IsNullable.Should().BeFalse();
            differences.Tables[0].Target.Columns[1].MaxLength.Should().Be(50);
            differences.Tables[0].Target.Columns[1].Name.Should().Be("Nullable");
            differences.Tables[0].Target.Columns[1].Position.Should().Be(2);
            differences.Tables[0].Target.Columns[1].Precision.Should().Be(0);
            differences.Tables[0].Target.Columns[1].Scale.Should().Be(0);
            differences.Tables[0].Target.Columns[1].SystemTypeId.Should().Be(167);
            differences.Tables[0].Target.Columns[1].TypeName.Should().Be("varchar");

            differences.Tables[0].Target.Columns[2].CollationName.Should().BeNull();
            differences.Tables[0].Target.Columns[2].ComputedExpression.Should().BeNull();
            differences.Tables[0].Target.Columns[2].IsComputed.Should().BeFalse();
            differences.Tables[0].Target.Columns[2].IsIdentity.Should().BeFalse();
            differences.Tables[0].Target.Columns[2].IsNullable.Should().BeFalse();
            differences.Tables[0].Target.Columns[2].MaxLength.Should().Be(5);
            differences.Tables[0].Target.Columns[2].Name.Should().Be("Precision");
            differences.Tables[0].Target.Columns[2].Position.Should().Be(3);
            differences.Tables[0].Target.Columns[2].Precision.Should().Be(5);
            differences.Tables[0].Target.Columns[2].Scale.Should().Be(2);
            differences.Tables[0].Target.Columns[2].SystemTypeId.Should().Be(106);
            differences.Tables[0].Target.Columns[2].TypeName.Should().Be("decimal");

            differences.Tables[0].Target.Columns[3].CollationName.Should().Be("SQL_Latin1_General_CP1_CI_AS");
            differences.Tables[0].Target.Columns[3].ComputedExpression.Should().BeNull();
            differences.Tables[0].Target.Columns[3].IsComputed.Should().BeFalse();
            differences.Tables[0].Target.Columns[3].IsIdentity.Should().BeFalse();
            differences.Tables[0].Target.Columns[3].IsNullable.Should().BeFalse();
            differences.Tables[0].Target.Columns[3].MaxLength.Should().Be(20);
            differences.Tables[0].Target.Columns[3].Name.Should().Be("MaxLength");
            differences.Tables[0].Target.Columns[3].Position.Should().Be(4);
            differences.Tables[0].Target.Columns[3].Precision.Should().Be(0);
            differences.Tables[0].Target.Columns[3].Scale.Should().Be(0);
            differences.Tables[0].Target.Columns[3].SystemTypeId.Should().Be(167);
            differences.Tables[0].Target.Columns[3].TypeName.Should().Be("varchar");

            differences.Tables[0].Target.Columns[4].CollationName.Should().BeNull();
            differences.Tables[0].Target.Columns[4].ComputedExpression.Should().BeNull();
            differences.Tables[0].Target.Columns[4].IsComputed.Should().BeFalse();
            differences.Tables[0].Target.Columns[4].IsIdentity.Should().BeFalse();
            differences.Tables[0].Target.Columns[4].IsNullable.Should().BeFalse();
            differences.Tables[0].Target.Columns[4].MaxLength.Should().Be(9);
            differences.Tables[0].Target.Columns[4].Name.Should().Be("Scale");
            differences.Tables[0].Target.Columns[4].Position.Should().Be(5);
            differences.Tables[0].Target.Columns[4].Precision.Should().Be(10);
            differences.Tables[0].Target.Columns[4].Scale.Should().Be(4);
            differences.Tables[0].Target.Columns[4].SystemTypeId.Should().Be(106);
            differences.Tables[0].Target.Columns[4].TypeName.Should().Be("decimal");

            differences.Tables[0].Target.Columns[5].CollationName.Should().BeNull();
            differences.Tables[0].Target.Columns[5].ComputedExpression.Should().BeNull();
            differences.Tables[0].Target.Columns[5].IsComputed.Should().BeFalse();
            differences.Tables[0].Target.Columns[5].IsIdentity.Should().BeFalse();
            differences.Tables[0].Target.Columns[5].IsNullable.Should().BeFalse();
            differences.Tables[0].Target.Columns[5].MaxLength.Should().Be(4);
            differences.Tables[0].Target.Columns[5].Name.Should().Be("Identity");
            differences.Tables[0].Target.Columns[5].Position.Should().Be(6);
            differences.Tables[0].Target.Columns[5].Precision.Should().Be(10);
            differences.Tables[0].Target.Columns[5].Scale.Should().Be(0);
            differences.Tables[0].Target.Columns[5].SystemTypeId.Should().Be(56);
            differences.Tables[0].Target.Columns[5].TypeName.Should().Be("int");

            differences.Tables[0].Target.Columns[6].CollationName.Should().BeNull();
            differences.Tables[0].Target.Columns[6].ComputedExpression.Should().BeNull();
            differences.Tables[0].Target.Columns[6].IsComputed.Should().BeFalse();
            differences.Tables[0].Target.Columns[6].IsIdentity.Should().BeFalse();
            differences.Tables[0].Target.Columns[6].IsNullable.Should().BeTrue();
            differences.Tables[0].Target.Columns[6].MaxLength.Should().Be(4);
            differences.Tables[0].Target.Columns[6].Name.Should().Be("ForeignKeyId");
            differences.Tables[0].Target.Columns[6].Position.Should().Be(7);
            differences.Tables[0].Target.Columns[6].Precision.Should().Be(10);
            differences.Tables[0].Target.Columns[6].Scale.Should().Be(0);
            differences.Tables[0].Target.Columns[6].SystemTypeId.Should().Be(56);
            differences.Tables[0].Target.Columns[6].TypeName.Should().Be("int");

            // Tables / Foreign keys
            differences.Tables[0].Source.ForeignKeys.Should().HaveCount(1);

            differences.Tables[0].Source.ForeignKeys[0].Columns.Should().HaveCount(1);
            differences.Tables[0].Source.ForeignKeys[0].Columns[0].Name.Should().Be("ForeignKeyId");
            differences.Tables[0].Source.ForeignKeys[0].Columns[0].Position.Should().Be(1);
            differences.Tables[0].Source.ForeignKeys[0].Columns[0].Referenced.Should().Be("Id");
            differences.Tables[0].Source.ForeignKeys[0].DeleteAction.Should().Be("NO_ACTION");
            differences.Tables[0].Source.ForeignKeys[0].Name.Should().Be("ForeignKeyDifference");
            differences.Tables[0].Source.ForeignKeys[0].UpdateAction.Should().Be("SET_NULL");

            differences.Tables[0].Target.ForeignKeys[0].Columns.Should().HaveCount(1);
            differences.Tables[0].Target.ForeignKeys[0].Columns[0].Name.Should().Be("ForeignKeyId");
            differences.Tables[0].Target.ForeignKeys[0].Columns[0].Position.Should().Be(1);
            differences.Tables[0].Target.ForeignKeys[0].Columns[0].Referenced.Should().Be("Id");
            differences.Tables[0].Target.ForeignKeys[0].DeleteAction.Should().Be("CASCADE");
            differences.Tables[0].Target.ForeignKeys[0].Name.Should().Be("ForeignKeyDifference");
            differences.Tables[0].Target.ForeignKeys[0].UpdateAction.Should().Be("CASCADE");

            differences.Tables[0].ForeignKeys.Should().HaveCount(1);
            differences.Tables[0].ForeignKeys[0].Columns.Should().HaveCount(0);
            differences.Tables[0].ForeignKeys[0].Properties.Should().HaveCount(2);
            differences.Tables[0].ForeignKeys[0].Properties[0].Name.Should().Be("DeleteAction");
            differences.Tables[0].ForeignKeys[0].Properties[0].Source.Should().Be("NO_ACTION");
            differences.Tables[0].ForeignKeys[0].Properties[0].Target.Should().Be("CASCADE");
            differences.Tables[0].ForeignKeys[0].Properties[1].Name.Should().Be("UpdateAction");
            differences.Tables[0].ForeignKeys[0].Properties[1].Source.Should().Be("SET_NULL");
            differences.Tables[0].ForeignKeys[0].Properties[1].Target.Should().Be("CASCADE");
            differences.Tables[0].ForeignKeys[0].Source.Should().BeSameAs(differences.Tables[0].Source.ForeignKeys[0]);
            differences.Tables[0].ForeignKeys[0].Target.Should().BeSameAs(differences.Tables[0].Target.ForeignKeys[0]);

            // Tables / Indexes
            differences.Tables[0].Source.Indexes.Should().HaveCount(2);

            differences.Tables[0].Source.Indexes[0].Columns.Should().HaveCount(2);
            differences.Tables[0].Source.Indexes[0].Columns[0].Name.Should().Be("ForeignKeyId");
            differences.Tables[0].Source.Indexes[0].Columns[0].Position.Should().Be(1);
            differences.Tables[0].Source.Indexes[0].Columns[1].Name.Should().Be("Type");
            differences.Tables[0].Source.Indexes[0].Columns[1].Position.Should().Be(2);
            differences.Tables[0].Source.Indexes[0].Filter.Should().Be("([Type]=(1234))");
            differences.Tables[0].Source.Indexes[0].IncludedColumns.Should().HaveCount(0);
            differences.Tables[0].Source.Indexes[0].IsUnique.Should().BeFalse();
            differences.Tables[0].Source.Indexes[0].Name.Should().Be("IndexDifference");
            differences.Tables[0].Source.Indexes[0].Type.Should().Be("NONCLUSTERED");
            differences.Tables[0].Source.Indexes[1].Columns.Should().HaveCount(2);
            differences.Tables[0].Source.Indexes[1].Columns[0].Name.Should().Be("Type");
            differences.Tables[0].Source.Indexes[1].Columns[0].Position.Should().Be(1);
            differences.Tables[0].Source.Indexes[1].Columns[1].Name.Should().Be("MaxLength");
            differences.Tables[0].Source.Indexes[1].Columns[1].Position.Should().Be(2);
            differences.Tables[0].Source.Indexes[1].Filter.Should().BeNull();
            differences.Tables[0].Source.Indexes[1].IncludedColumns.Should().HaveCount(0);
            differences.Tables[0].Source.Indexes[1].IsUnique.Should().BeTrue();
            differences.Tables[0].Source.Indexes[1].Name.Should().Be("PrimaryKeyDifference");
            differences.Tables[0].Source.Indexes[1].Type.Should().Be("CLUSTERED");

            differences.Tables[0].Target.Indexes.Should().HaveCount(2);
            differences.Tables[0].Target.Indexes[0].Columns.Should().HaveCount(2);
            differences.Tables[0].Target.Indexes[0].Columns[0].Name.Should().Be("Type");
            differences.Tables[0].Target.Indexes[0].Columns[0].Position.Should().Be(1);
            differences.Tables[0].Target.Indexes[0].Columns[1].Name.Should().Be("ForeignKeyId");
            differences.Tables[0].Target.Indexes[0].Columns[1].Position.Should().Be(2);
            differences.Tables[0].Target.Indexes[0].Filter.Should().Be("([Type]='Target')");
            differences.Tables[0].Target.Indexes[0].IncludedColumns.Should().HaveCount(0);
            differences.Tables[0].Target.Indexes[0].IsUnique.Should().BeFalse();
            differences.Tables[0].Target.Indexes[0].Name.Should().Be("IndexDifference");
            differences.Tables[0].Target.Indexes[0].Type.Should().Be("NONCLUSTERED");
            differences.Tables[0].Target.Indexes[1].Columns.Should().HaveCount(2);
            differences.Tables[0].Target.Indexes[1].Columns[0].Name.Should().Be("MaxLength");
            differences.Tables[0].Target.Indexes[1].Columns[0].Position.Should().Be(1);
            differences.Tables[0].Target.Indexes[1].Columns[1].Name.Should().Be("Type");
            differences.Tables[0].Target.Indexes[1].Columns[1].Position.Should().Be(2);
            differences.Tables[0].Target.Indexes[1].Filter.Should().BeNull();
            differences.Tables[0].Target.Indexes[1].IncludedColumns.Should().HaveCount(0);
            differences.Tables[0].Target.Indexes[1].IsUnique.Should().BeTrue();
            differences.Tables[0].Target.Indexes[1].Name.Should().Be("PrimaryKeyDifference");
            differences.Tables[0].Target.Indexes[1].Type.Should().Be("CLUSTERED");

            differences.Tables[0].Indexes.Should().HaveCount(2);

            differences.Tables[0].Indexes[0].Properties.Should().HaveCount(1);
            differences.Tables[0].Indexes[0].Properties[0].Name.Should().Be("Filter");
            differences.Tables[0].Indexes[0].Properties[0].Source.Should().Be("([Type]=(1234))");
            differences.Tables[0].Indexes[0].Properties[0].Target.Should().Be("([Type]='Target')");
            differences.Tables[0].Indexes[0].Source.Should().BeSameAs(differences.Tables[0].Source.Indexes[0]);
            differences.Tables[0].Indexes[0].Target.Should().BeSameAs(differences.Tables[0].Target.Indexes[0]);
            differences.Tables[0].Indexes[0].Type.Should().Be(SqlObjectDifferenceType.Different);

            differences.Tables[0].Indexes[1].Properties.Should().HaveCount(0);
            differences.Tables[0].Indexes[1].Source.Should().BeSameAs(differences.Tables[0].Source.Indexes[1]);
            differences.Tables[0].Indexes[1].Target.Should().BeSameAs(differences.Tables[0].Target.Indexes[1]);
            differences.Tables[0].Indexes[1].Type.Should().Be(SqlObjectDifferenceType.Different);

            // Tables / Primary keys
            differences.Tables[0].Source.PrimaryKey.Name.Should().Be("PrimaryKeyDifference");
            differences.Tables[0].Source.PrimaryKey.Type.Should().Be("CLUSTERED");
            differences.Tables[0].Source.PrimaryKey.Columns.Should().HaveCount(2);
            differences.Tables[0].Source.PrimaryKey.Columns[0].Name.Should().Be("Type");
            differences.Tables[0].Source.PrimaryKey.Columns[0].Position.Should().Be(1);
            differences.Tables[0].Source.PrimaryKey.Columns[1].Name.Should().Be("MaxLength");
            differences.Tables[0].Source.PrimaryKey.Columns[1].Position.Should().Be(2);

            differences.Tables[0].Target.PrimaryKey.Name.Should().Be("PrimaryKeyDifference");
            differences.Tables[0].Target.PrimaryKey.Type.Should().Be("CLUSTERED");
            differences.Tables[0].Target.PrimaryKey.Columns.Should().HaveCount(2);
            differences.Tables[0].Target.PrimaryKey.Columns[0].Name.Should().Be("MaxLength");
            differences.Tables[0].Target.PrimaryKey.Columns[0].Position.Should().Be(1);
            differences.Tables[0].Target.PrimaryKey.Columns[1].Name.Should().Be("Type");
            differences.Tables[0].Target.PrimaryKey.Columns[1].Position.Should().Be(2);

            differences.Tables[0].PrimaryKey.Columns.Should().HaveCount(2);
            differences.Tables[0].PrimaryKey.Columns[0].Source.Should().BeSameAs(differences.Tables[0].Source.PrimaryKey.Columns[1]);
            differences.Tables[0].PrimaryKey.Columns[0].Target.Should().BeSameAs(differences.Tables[0].Target.PrimaryKey.Columns[0]);
            differences.Tables[0].PrimaryKey.Columns[0].Type.Should().Be(SqlObjectDifferenceType.Different);
            differences.Tables[0].PrimaryKey.Columns[1].Source.Should().BeSameAs(differences.Tables[0].Source.PrimaryKey.Columns[0]);
            differences.Tables[0].PrimaryKey.Columns[1].Target.Should().BeSameAs(differences.Tables[0].Target.PrimaryKey.Columns[1]);
            differences.Tables[0].PrimaryKey.Columns[1].Type.Should().Be(SqlObjectDifferenceType.Different);

            differences.Tables[0].PrimaryKey.Properties.Should().BeEmpty();
            differences.Tables[0].PrimaryKey.Source.Should().Be(differences.Tables[0].Source.PrimaryKey);
            differences.Tables[0].PrimaryKey.Target.Should().Be(differences.Tables[0].Target.PrimaryKey);
            differences.Tables[0].PrimaryKey.Type.Should().Be(SqlObjectDifferenceType.Different);

            // Tables / Triggers
            differences.Tables[0].Source.Triggers.Should().HaveCount(1);

            differences.Tables[0].Source.Triggers[0].Name.Should().Be("TriggerDifference");
            differences.Tables[0].Source.Triggers[0].Code.Should().Be("CREATE TRIGGER [TriggerDifference]\r\n\tON [dbo].[TableDifference]\r\n\tINSTEAD OF INSERT\r\n\tAS\r\n\tBEGIN\r\n\t\tPRINT 'From source'\r\n\tEND");
            differences.Tables[0].Source.Triggers[0].IsInsteadOfTrigger.Should().BeTrue();

            differences.Tables[0].Target.Triggers.Should().HaveCount(1);

            differences.Tables[0].Target.Triggers[0].Name.Should().Be("TriggerDifference");
            differences.Tables[0].Target.Triggers[0].Code.Should().Be("CREATE TRIGGER [TriggerDifference]\r\n\tON [dbo].[TableDifference]\r\n\tFOR INSERT\r\n\tAS\r\n\tBEGIN\r\n\t\tPRINT 'From target'\r\n\tEND");
            differences.Tables[0].Target.Triggers[0].IsInsteadOfTrigger.Should().BeFalse();

            differences.Tables[0].Triggers.Should().HaveCount(1);
            differences.Tables[0].Triggers[0].Properties.Should().HaveCount(2);
            differences.Tables[0].Triggers[0].Properties[0].Name.Should().Be("IsInsteadOfTrigger");
            differences.Tables[0].Triggers[0].Properties[0].Source.Should().Be(true);
            differences.Tables[0].Triggers[0].Properties[0].Target.Should().Be(false);
            differences.Tables[0].Triggers[0].Properties[1].Name.Should().Be("Code");
            differences.Tables[0].Triggers[0].Properties[1].Source.Should().Be("CREATE TRIGGER [TriggerDifference]\r\n\tON [dbo].[TableDifference]\r\n\tINSTEAD OF INSERT\r\n\tAS\r\n\tBEGIN\r\n\t\tPRINT 'From source'\r\n\tEND");
            differences.Tables[0].Triggers[0].Properties[1].Target.Should().Be("CREATE TRIGGER [TriggerDifference]\r\n\tON [dbo].[TableDifference]\r\n\tFOR INSERT\r\n\tAS\r\n\tBEGIN\r\n\t\tPRINT 'From target'\r\n\tEND");
            differences.Tables[0].Triggers[0].Source.Should().BeSameAs(differences.Tables[0].Source.Triggers[0]);
            differences.Tables[0].Triggers[0].Target.Should().BeSameAs(differences.Tables[0].Target.Triggers[0]);
            differences.Tables[0].Triggers[0].Type.Should().Be(SqlObjectDifferenceType.Different);

            // Tables / Unique constraints
            differences.Tables[0].Source.UniqueConstraints.Should().HaveCount(1);
            differences.Tables[0].Source.UniqueConstraints[0].Columns.Should().HaveCount(2);
            differences.Tables[0].Source.UniqueConstraints[0].Columns[0].Name.Should().Be("Type");
            differences.Tables[0].Source.UniqueConstraints[0].Columns[0].Position.Should().Be(1);
            differences.Tables[0].Source.UniqueConstraints[0].Columns[1].Name.Should().Be("MaxLength");
            differences.Tables[0].Source.UniqueConstraints[0].Columns[1].Position.Should().Be(2);
            differences.Tables[0].Source.UniqueConstraints[0].Name.Should().Be("UniqueConstraintDifference");
            differences.Tables[0].Source.UniqueConstraints[0].Type.Should().Be("NONCLUSTERED");

            differences.Tables[0].Target.UniqueConstraints.Should().HaveCount(1);
            differences.Tables[0].Target.UniqueConstraints[0].Columns.Should().HaveCount(2);
            differences.Tables[0].Target.UniqueConstraints[0].Columns[0].Name.Should().Be("MaxLength");
            differences.Tables[0].Target.UniqueConstraints[0].Columns[0].Position.Should().Be(1);
            differences.Tables[0].Target.UniqueConstraints[0].Columns[1].Name.Should().Be("Type");
            differences.Tables[0].Target.UniqueConstraints[0].Columns[1].Position.Should().Be(2);
            differences.Tables[0].Target.UniqueConstraints[0].Name.Should().Be("UniqueConstraintDifference");
            differences.Tables[0].Target.UniqueConstraints[0].Type.Should().Be("NONCLUSTERED");

            differences.Tables[0].UniqueConstraints.Should().HaveCount(1);
            differences.Tables[0].UniqueConstraints[0].Properties.Should().HaveCount(0);
            differences.Tables[0].UniqueConstraints[0].Source.Should().BeSameAs(differences.Tables[0].Source.UniqueConstraints[0]);
            differences.Tables[0].UniqueConstraints[0].Target.Should().BeSameAs(differences.Tables[0].Target.UniqueConstraints[0]);
            differences.Tables[0].UniqueConstraints[0].Type.Should().Be(SqlObjectDifferenceType.Different);

            // Missing tables
            differences.Tables[1].Columns.Should().BeNull();
            differences.Tables[1].Indexes.Should().BeNull();
            differences.Tables[1].PrimaryKey.Should().BeNull();
            differences.Tables[1].Source.Should().BeNull();
            differences.Tables[1].UniqueConstraints.Should().BeNull();
            differences.Tables[1].Target.CheckConstraints.Should().HaveCount(1);
            differences.Tables[1].Target.CheckConstraints[0].Name.Should().Be("CheckConstraintTarget");
            differences.Tables[1].Target.CheckConstraints[0].Code.Should().Be("([Id]>(0))");
            differences.Tables[1].Target.Columns.Should().HaveCount(3);
            differences.Tables[1].Target.Columns[0].CollationName.Should().BeNull();
            differences.Tables[1].Target.Columns[0].ComputedExpression.Should().BeNull();
            differences.Tables[1].Target.Columns[0].IsComputed.Should().BeFalse();
            differences.Tables[1].Target.Columns[0].IsIdentity.Should().BeFalse();
            differences.Tables[1].Target.Columns[0].IsNullable.Should().BeFalse();
            differences.Tables[1].Target.Columns[0].MaxLength.Should().Be(4);
            differences.Tables[1].Target.Columns[0].Name.Should().Be("Id");
            differences.Tables[1].Target.Columns[0].Position.Should().Be(1);
            differences.Tables[1].Target.Columns[0].Precision.Should().Be(10);
            differences.Tables[1].Target.Columns[0].Scale.Should().Be(0);
            differences.Tables[1].Target.Columns[0].SystemTypeId.Should().Be(56);
            differences.Tables[1].Target.Columns[0].TypeName.Should().Be("int");
            differences.Tables[1].Target.ForeignKeys.Should().HaveCount(1);
            differences.Tables[1].Target.ForeignKeys[0].Columns.Should().HaveCount(1);
            differences.Tables[1].Target.ForeignKeys[0].Columns[0].Name.Should().Be("TargetForeignKeyId");
            differences.Tables[1].Target.ForeignKeys[0].Columns[0].Position.Should().Be(1);
            differences.Tables[1].Target.ForeignKeys[0].DeleteAction.Should().Be("NO_ACTION");
            differences.Tables[1].Target.ForeignKeys[0].Name.Should().Be("ForeignKeyTarget");
            differences.Tables[1].Target.ForeignKeys[0].ReferencedTable.Should().Be("ReferencedTable");
            differences.Tables[1].Target.ForeignKeys[0].UpdateAction.Should().Be("NO_ACTION");
            differences.Tables[1].Target.Indexes.Should().HaveCount(2);
            differences.Tables[1].Target.Indexes[0].Columns.Should().HaveCount(1);
            differences.Tables[1].Target.Indexes[0].Columns[0].Name.Should().Be("TargetName");
            differences.Tables[1].Target.Indexes[0].Columns[0].Position.Should().Be(1);
            differences.Tables[1].Target.Indexes[0].Filter.Should().Be("([TargetName]='')");
            differences.Tables[1].Target.Indexes[0].IncludedColumns.Should().HaveCount(0);
            differences.Tables[1].Target.Indexes[0].IsUnique.Should().BeFalse();
            differences.Tables[1].Target.Indexes[0].Name.Should().Be("IndexTarget");
            differences.Tables[1].Target.Indexes[1].Columns.Should().HaveCount(1);
            differences.Tables[1].Target.Indexes[1].Columns[0].Name.Should().Be("Id");
            differences.Tables[1].Target.Indexes[1].Columns[0].Position.Should().Be(1);
            differences.Tables[1].Target.Indexes[1].Filter.Should().BeNull();
            differences.Tables[1].Target.Indexes[1].IncludedColumns.Should().HaveCount(0);
            differences.Tables[1].Target.Name.Should().Be("TableTarget");
            differences.Tables[1].Target.PrimaryKey.Name.Should().Be("PrimaryKeyTarget");
            differences.Tables[1].Target.PrimaryKey.Type.Should().Be("CLUSTERED");
            differences.Tables[1].Target.Schema.Should().Be("dbo");
            differences.Tables[1].Target.Triggers.Should().HaveCount(1);
            differences.Tables[1].Target.Triggers[0].Name.Should().Be("TriggerTarget");
            differences.Tables[1].Target.Triggers[0].Code.Should().Be("CREATE TRIGGER [TriggerTarget]\r\n\tON [dbo].[TableTarget]\r\n\tFOR DELETE, INSERT, UPDATE\r\n\tAS\r\n\tBEGIN\r\n\t\tSET NOCOUNT ON\r\n\tEND");
            differences.Tables[1].Target.Triggers[0].IsInsteadOfTrigger.Should().BeFalse();
            differences.Tables[1].Target.UniqueConstraints.Should().HaveCount(1);
            differences.Tables[1].Target.UniqueConstraints[0].Columns.Should().HaveCount(1);
            differences.Tables[1].Target.UniqueConstraints[0].Columns[0].Name.Should().Be("Id");
            differences.Tables[1].Target.UniqueConstraints[0].Columns[0].Position.Should().Be(1);
            differences.Tables[1].Target.UniqueConstraints[0].Name.Should().Be("UniqueConstraintTarget");
            differences.Tables[1].Target.UniqueConstraints[0].Type.Should().Be("NONCLUSTERED");
            differences.Tables[1].Triggers.Should().BeNull();
            differences.Tables[1].Type.Should().Be(SqlObjectDifferenceType.MissingInSource);

            differences.Tables[2].Columns.Should().BeNull();
            differences.Tables[2].Indexes.Should().BeNull();
            differences.Tables[2].PrimaryKey.Should().BeNull();
            differences.Tables[2].UniqueConstraints.Should().BeNull();
            differences.Tables[2].Source.CheckConstraints.Should().HaveCount(1);
            differences.Tables[2].Source.CheckConstraints[0].Name.Should().Be("CheckConstraintSource");
            differences.Tables[2].Source.CheckConstraints[0].Code.Should().Be("([Id]>(0))");
            differences.Tables[2].Source.Columns.Should().HaveCount(3);
            differences.Tables[2].Source.Columns[0].CollationName.Should().BeNull();
            differences.Tables[2].Source.Columns[0].ComputedExpression.Should().BeNull();
            differences.Tables[2].Source.Columns[0].IsComputed.Should().BeFalse();
            differences.Tables[2].Source.Columns[0].IsIdentity.Should().BeFalse();
            differences.Tables[2].Source.Columns[0].IsNullable.Should().BeFalse();
            differences.Tables[2].Source.Columns[0].MaxLength.Should().Be(4);
            differences.Tables[2].Source.Columns[0].Name.Should().Be("Id");
            differences.Tables[2].Source.Columns[0].Position.Should().Be(1);
            differences.Tables[2].Source.Columns[0].Precision.Should().Be(10);
            differences.Tables[2].Source.Columns[0].Scale.Should().Be(0);
            differences.Tables[2].Source.Columns[0].SystemTypeId.Should().Be(56);
            differences.Tables[2].Source.Columns[0].TypeName.Should().Be("int");
            differences.Tables[2].Source.Indexes.Should().HaveCount(2);
            differences.Tables[2].Source.Indexes[0].Columns.Should().HaveCount(1);
            differences.Tables[2].Source.Indexes[0].Columns[0].Name.Should().Be("SourceName");
            differences.Tables[2].Source.Indexes[0].Columns[0].Position.Should().Be(1);
            differences.Tables[2].Source.Indexes[0].Filter.Should().Be("([SourceName]='')");
            differences.Tables[2].Source.Indexes[0].IncludedColumns.Should().HaveCount(0);
            differences.Tables[2].Source.Indexes[0].IsUnique.Should().BeFalse();
            differences.Tables[2].Source.Indexes[0].Name.Should().Be("IndexSource");
            differences.Tables[2].Source.Indexes[1].Columns.Should().HaveCount(1);
            differences.Tables[2].Source.Indexes[1].Columns[0].Name.Should().Be("Id");
            differences.Tables[2].Source.Indexes[1].Columns[0].Position.Should().Be(1);
            differences.Tables[2].Source.Indexes[1].Filter.Should().BeNull();
            differences.Tables[2].Source.Indexes[1].IncludedColumns.Should().HaveCount(0);
            differences.Tables[2].Source.ForeignKeys.Should().HaveCount(1);
            differences.Tables[2].Source.ForeignKeys[0].Columns.Should().HaveCount(1);
            differences.Tables[2].Source.ForeignKeys[0].Columns[0].Name.Should().Be("SourceForeignKeyId");
            differences.Tables[2].Source.ForeignKeys[0].Columns[0].Position.Should().Be(1);
            differences.Tables[2].Source.ForeignKeys[0].DeleteAction.Should().Be("NO_ACTION");
            differences.Tables[2].Source.ForeignKeys[0].Name.Should().Be("ForeignKeySource");
            differences.Tables[2].Source.ForeignKeys[0].ReferencedTable.Should().Be("ReferencedTable");
            differences.Tables[2].Source.ForeignKeys[0].UpdateAction.Should().Be("NO_ACTION");
            differences.Tables[2].Source.Name.Should().Be("TableSource");
            differences.Tables[2].Source.Schema.Should().Be("dbo");
            differences.Tables[2].Source.PrimaryKey.Name.Should().Be("PrimaryKeySource");
            differences.Tables[2].Source.PrimaryKey.Type.Should().Be("CLUSTERED");
            differences.Tables[2].Source.Triggers.Should().HaveCount(1);
            differences.Tables[2].Source.Triggers[0].Name.Should().Be("TriggerSource");
            differences.Tables[2].Source.Triggers[0].Code.Should().Be("CREATE TRIGGER [TriggerSource]\r\n\tON [dbo].[TableSource]\r\n\tFOR DELETE, INSERT, UPDATE\r\n\tAS\r\n\tBEGIN\r\n\t\tSET NOCOUNT ON\r\n\tEND");
            differences.Tables[2].Source.Triggers[0].IsInsteadOfTrigger.Should().BeFalse();
            differences.Tables[2].Source.UniqueConstraints.Should().HaveCount(1);
            differences.Tables[2].Source.UniqueConstraints[0].Columns.Should().HaveCount(1);
            differences.Tables[2].Source.UniqueConstraints[0].Columns[0].Name.Should().Be("Id");
            differences.Tables[2].Source.UniqueConstraints[0].Columns[0].Position.Should().Be(1);
            differences.Tables[2].Source.UniqueConstraints[0].Name.Should().Be("UniqueConstraintSource");
            differences.Tables[2].Source.UniqueConstraints[0].Type.Should().Be("NONCLUSTERED");
            differences.Tables[2].Triggers.Should().BeNull();
            differences.Tables[2].Type.Should().Be(SqlObjectDifferenceType.MissingInTarget);

            // UserTypes
            differences.UserTypes.Should().HaveCount(3);

            differences.UserTypes[0].Source.Name.Should().Be("TypeDifference");
            differences.UserTypes[0].Target.Name.Should().Be("TypeDifference");
            differences.UserTypes[0].Properties.Should().HaveCount(3);

            differences.UserTypes[0].Properties[0].Name.Should().Be("IsNullable");
            differences.UserTypes[0].Properties[0].Source.Should().Be(false);
            differences.UserTypes[0].Properties[0].Target.Should().Be(true);

            differences.UserTypes[0].Properties[1].Name.Should().Be("MaxLength");
            differences.UserTypes[0].Properties[1].Source.Should().Be(11);
            differences.UserTypes[0].Properties[1].Target.Should().Be(4);

            differences.UserTypes[0].Properties[2].Name.Should().Be("SystemTypeId");
            differences.UserTypes[0].Properties[2].Source.Should().Be(167);
            differences.UserTypes[0].Properties[2].Target.Should().Be(56);

            differences.UserTypes[1].Source.Should().BeNull();
            differences.UserTypes[1].Target.Name.Should().Be("TypeTarget");
            differences.UserTypes[1].Type.Should().Be(SqlObjectDifferenceType.MissingInSource);

            differences.UserTypes[2].Source.Name.Should().Be("TypeSource");
            differences.UserTypes[2].Target.Should().BeNull();
            differences.UserTypes[2].Type.Should().Be(SqlObjectDifferenceType.MissingInTarget);

            // Views
            differences.Views.Should().HaveCount(3);

            differences.Views[0].Source.Name.Should().Be("ViewDifference");
            differences.Views[0].Source.Schema.Should().Be("dbo");
            differences.Views[0].Target.Name.Should().Be("ViewDifference");
            differences.Views[0].Target.Schema.Should().Be("dbo");
            differences.Views[0].Properties.Should().HaveCount(1);
            differences.Views[0].Properties[0].Name.Should().Be("Code");
            differences.Views[0].Properties[0].Source.Should().Be("CREATE VIEW [dbo].[ViewDifference]\r\n\tAS SELECT * FROM [TableDifference] WHERE [Type] = 10");
            differences.Views[0].Properties[0].Target.Should().Be("CREATE VIEW [dbo].[ViewDifference]\r\n\tAS SELECT * FROM [TableDifference] WHERE [Type] = 'The type'");

            differences.Views[1].Source.Should().BeNull();
            differences.Views[1].Target.Name.Should().Be("ViewTarget");
            differences.Views[1].Target.Schema.Should().Be("dbo");
            differences.Views[1].Type.Should().Be(SqlObjectDifferenceType.MissingInSource);

            differences.Views[2].Source.Name.Should().Be("ViewSource");
            differences.Views[2].Source.Schema.Should().Be("dbo");
            differences.Views[2].Target.Should().BeNull();
            differences.Views[2].Type.Should().Be(SqlObjectDifferenceType.MissingInTarget);
        }
    }
}