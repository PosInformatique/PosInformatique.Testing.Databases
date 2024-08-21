//-----------------------------------------------------------------------
// <copyright file="SqlServerDatabaseComparerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer.Tests
{
    public class SqlServerDatabaseComparerTest
    {
        private const string ConnectionString = $"Data Source=(localDB)\\posinfo-unit-tests; Integrated Security=True";

        [Fact]
        public void Constructor()
        {
            var server = new SqlServer(ConnectionString);

            var sourceDatabase = server.DeployDacPackage("UnitTests.Databases.SqlServer.Tests.Source.dacpac", $"{nameof(SqlServerDatabaseComparerTest)}_Source");
            var targetDatabase = server.DeployDacPackage("UnitTests.Databases.SqlServer.Tests.Target.dacpac", $"{nameof(SqlServerDatabaseComparerTest)}_Target");

            var comparer = new SqlServerDatabaseComparer();

            var differences = comparer.Compare(sourceDatabase, targetDatabase);

            differences.Objects.Should().HaveCount(10);

            // Types
            differences.Objects[0].Type.Should().Be("Types");

            differences.Objects[0].Differences.Should().HaveCount(3);
            differences.Objects[0].Differences[0].ToString().Should().Be("TypeDifference:\r\n- SystemTypeId: (Source: 56, Target: 167)\r\n- MaxLength: (Source: 4, Target: 11)");
            differences.Objects[0].Differences[1].ToString().Should().Be("(Missing) <=> TypeTarget");
            differences.Objects[0].Differences[2].ToString().Should().Be("TypeSource <=> (Missing)");

            // Columns
            differences.Objects[1].Type.Should().Be("Columns");

            differences.Objects[1].Differences.Should().HaveCount(12);
            differences.Objects[1].Differences[0].ToString().Should().Be("TableDifference.Type:\r\n- SystemTypeId: (Source: 167, Target: 56)\r\n- TypeName: (Source: varchar, Target: int)\r\n- MaxLength: (Source: 50, Target: 4)\r\n- Precision: (Source: 0, Target: 10)\r\n- CollationName: (Source: SQL_Latin1_General_CP1_CI_AS, Target: )");
            differences.Objects[1].Differences[1].ToString().Should().Be("TableDifference.Nullable:\r\n- IsNullable: (Source: False, Target: True)");
            differences.Objects[1].Differences[2].ToString().Should().Be("TableDifference.Precision:\r\n- Position: (Source: 3, Target: 4)\r\n- MaxLength: (Source: 5, Target: 9)\r\n- Precision: (Source: 5, Target: 10)");
            differences.Objects[1].Differences[3].ToString().Should().Be("TableDifference.MaxLength:\r\n- Position: (Source: 4, Target: 3)\r\n- MaxLength: (Source: 20, Target: 50)");
            differences.Objects[1].Differences[4].ToString().Should().Be("TableDifference.Scale:\r\n- Scale: (Source: 4, Target: 2)");
            differences.Objects[1].Differences[5].ToString().Should().Be("TableDifference.Identity:\r\n- IsIdentity: (Source: False, Target: True)");
            differences.Objects[1].Differences[6].ToString().Should().Be("(Missing) <=> TableTarget.Id");
            differences.Objects[1].Differences[7].ToString().Should().Be("(Missing) <=> TableTarget.TargetName");
            differences.Objects[1].Differences[8].ToString().Should().Be("(Missing) <=> TableTarget.TargetForeignKeyId");
            differences.Objects[1].Differences[9].ToString().Should().Be("TableSource.Id <=> (Missing)");
            differences.Objects[1].Differences[10].ToString().Should().Be("TableSource.SourceName <=> (Missing)");
            differences.Objects[1].Differences[11].ToString().Should().Be("TableSource.SourceForeignKeyId <=> (Missing)");

            // PrimaryKeys
            differences.Objects[2].Type.Should().Be("PrimaryKeys");

            differences.Objects[2].Differences.Should().HaveCount(3);
            differences.Objects[2].Differences[0].ToString().Should().Be("TableDifference.PrimaryKeyDifference.MaxLength:\r\n- Position: (Source: 4, Target: 3)");
            differences.Objects[2].Differences[1].ToString().Should().Be("(Missing) <=> TableTarget.PrimaryKeyTarget.Id");
            differences.Objects[2].Differences[2].ToString().Should().Be("TableSource.PrimaryKeySource.Id <=> (Missing)");

            // UniqueConstraints
            differences.Objects[3].Type.Should().Be("UniqueConstraints");

            differences.Objects[3].Differences.Should().HaveCount(3);
            differences.Objects[3].Differences[0].ToString().Should().Be("TableDifference.UniqueConstraintDifference.MaxLength:\r\n- Position: (Source: 4, Target: 3)");
            differences.Objects[3].Differences[1].ToString().Should().Be("(Missing) <=> TableTarget.UniqueConstraintTarget.Id");
            differences.Objects[3].Differences[2].ToString().Should().Be("TableSource.UniqueConstraintSource.Id <=> (Missing)");

            // ForeignKeys
            differences.Objects[4].Type.Should().Be("ForeignKeys");

            differences.Objects[4].Differences.Should().HaveCount(3);
            differences.Objects[4].Differences[0].ToString().Should().Be("ForeignKeyDifference:\r\n- DeleteAction: (Source: CASCADE, Target: NO_ACTION)\r\n- UpdateAction: (Source: CASCADE, Target: SET_NULL)");
            differences.Objects[4].Differences[1].ToString().Should().Be("(Missing) <=> ForeignKeyTarget");
            differences.Objects[4].Differences[2].ToString().Should().Be("ForeignKeySource <=> (Missing)");

            // Indexes
            differences.Objects[5].Type.Should().Be("Indexes");

            differences.Objects[5].Differences.Should().HaveCount(6);
            differences.Objects[5].Differences[0].ToString().Should().Be("TableDifference.IndexDifference.Type:\r\n- Position: (Source: 1, Target: 2)");
            differences.Objects[5].Differences[1].ToString().Should().Be("TableDifference.IndexDifference.ForeignKeyId:\r\n- Position: (Source: 2, Target: 1)");
            differences.Objects[5].Differences[2].ToString().Should().Be("(Missing) <=> TableTarget.IndexTarget.TargetName");
            differences.Objects[5].Differences[3].ToString().Should().Be("(Missing) <=> TableTarget.PrimaryKeyTarget.Id");
            differences.Objects[5].Differences[4].ToString().Should().Be("TableSource.IndexSource.SourceName <=> (Missing)");
            differences.Objects[5].Differences[5].ToString().Should().Be("TableSource.PrimaryKeySource.Id <=> (Missing)");

            // CheckConstraints
            differences.Objects[6].Type.Should().Be("CheckConstraints");

            differences.Objects[6].Differences.Should().HaveCount(3);
            differences.Objects[6].Differences[0].ToString().Should().Be("CheckConstraintDifference:\r\n- Definition: (Source: ([Type]=(2) OR [Type]=(1)), Target: ([Type]>(0)))");
            differences.Objects[6].Differences[1].ToString().Should().Be("(Missing) <=> CheckConstraintTarget");
            differences.Objects[6].Differences[2].ToString().Should().Be("CheckConstraintSource <=> (Missing)");

            // StoredProcedures
            differences.Objects[7].Type.Should().Be("StoredProcedures");

            differences.Objects[7].Differences.Should().HaveCount(3);
            differences.Objects[7].Differences[0].ToString().Should().Be("dbo.StoredProcedureDifference:\r\n- definition: (Source: CREATEPROCEDURE[dbo].[StoredProcedureDifference]@param1int=0,@param2intASSELECT@param1RETURN0, Target: CREATEPROCEDURE[dbo].[StoredProcedureDifference]@param1int=0,@param2intASSELECT@param2RETURN0)");
            differences.Objects[7].Differences[1].ToString().Should().Be("(Missing) <=> dbo.StoredProcedureTarget");
            differences.Objects[7].Differences[2].ToString().Should().Be("dbo.StoredProcedureSource <=> (Missing)");

            // Triggers
            differences.Objects[8].Type.Should().Be("Triggers");

            differences.Objects[8].Differences.Should().HaveCount(3);
            differences.Objects[8].Differences[0].ToString().Should().Be("TableDifference.TriggerDifference:\r\n- IsInsteadOfTrigger: (Source: False, Target: True)\r\n- Column1: (Source: CREATETRIGGER[TriggerDifference]ON[dbo].[TableDifference]FORINSERTASBEGINPRINT'Fromtarget'END, Target: CREATETRIGGER[TriggerDifference]ON[dbo].[TableDifference]INSTEADOFINSERTASBEGINPRINT'Fromsource'END)");
            differences.Objects[8].Differences[1].ToString().Should().Be("(Missing) <=> TableTarget.TriggerTarget");
            differences.Objects[8].Differences[2].ToString().Should().Be("TableSource.TriggerSource <=> (Missing)");

            // Views
            differences.Objects[9].Type.Should().Be("Views");

            differences.Objects[9].Differences.Should().HaveCount(3);
            differences.Objects[9].Differences[0].ToString().Should().Be("dbo.ViewDifference:\r\n- definition: (Source: CREATEVIEW[dbo].[ViewDifference]ASSELECT*FROM[TableDifference]WHERE[Type]='Thetype', Target: CREATEVIEW[dbo].[ViewDifference]ASSELECT*FROM[TableDifference]WHERE[Type]=10)");
            differences.Objects[9].Differences[1].ToString().Should().Be("(Missing) <=> dbo.ViewTarget");
            differences.Objects[9].Differences[2].ToString().Should().Be("dbo.ViewSource <=> (Missing)");
        }
    }
}