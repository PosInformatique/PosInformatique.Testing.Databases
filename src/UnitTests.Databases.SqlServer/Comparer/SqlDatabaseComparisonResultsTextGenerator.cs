//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseComparisonResultsTextGenerator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases
{
    internal sealed class SqlDatabaseComparisonResultsTextGenerator : ISqlDatabaseObjectDifferencesVisitor, IDisposable
    {
        private readonly StringWriter writer;

        private int tab;

        private bool writeTab;

        private SqlDatabaseComparisonResultsTextGenerator()
        {
            this.writer = new StringWriter();
        }

        public static string Generate(SqlDatabaseComparisonResults results)
        {
            using var generator = new SqlDatabaseComparisonResultsTextGenerator();

            generator.Generate(results.Tables, "Tables");
            generator.Generate(results.StoredProcedures, "Stored procedures");
            generator.Generate(results.UserTypes, "User types");
            generator.Generate(results.Views, "Views");

            return generator.writer.ToString();
        }

        public static string Generate<TSqlObject>(SqlDatabaseObjectDifferences<TSqlObject> differences)
            where TSqlObject : SqlObject
        {
            using var generator = new SqlDatabaseComparisonResultsTextGenerator();

            differences.Accept(generator);

            return generator.writer.ToString();
        }

        public static string Generate(SqlObjectPropertyDifference difference)
        {
            using var generator = new SqlDatabaseComparisonResultsTextGenerator();

            generator.WriteProperty(difference);

            return generator.writer.ToString();
        }

        public void Dispose()
        {
            this.writer.Dispose();
        }

        public void Visit<TSqlObject>(SqlDatabaseObjectDifferences<TSqlObject> differences)
            where TSqlObject : SqlObject
        {
            if (differences.Source is not null && differences.Target is not null)
            {
                this.WriteProperties(differences.Properties);
            }
        }

        public void Visit(SqlForeignKeyDifferences differences)
        {
            this.WriteProperties(differences.Properties);

            this.Generate(differences.Columns, "Columns");
        }

        public void Visit(SqlIndexDifferences differences)
        {
            this.WriteProperties(differences.Properties);

            this.Generate(differences.Columns, "Columns");
            this.Generate(differences.IncludedColumns, "Included columns");
        }

        public void Visit(SqlPrimaryKeyDifferences differences)
        {
            this.WriteProperties(differences.Properties);

            this.Generate(differences.Columns, "Columns");
        }

        public void Visit(SqlDatabaseTableDifferences differences)
        {
            if (differences.Source is not null && differences.Target is not null)
            {
                this.Indent();
                this.Generate(differences.CheckConstraints, "Check constraints");
                this.Unindent();

                this.Indent();
                this.Generate(differences.Columns, "Columns");
                this.Unindent();

                this.Indent();
                this.Generate(differences.ForeignKeys, "Foreign keys");
                this.Unindent();

                this.Indent();
                this.Generate(differences.Indexes, "Indexes");
                this.Unindent();

                if (differences.PrimaryKey is not null)
                {
                    this.Indent();
                    this.WriteLine($"------ Primary key ------");
                    differences.PrimaryKey.Accept(this);
                    this.Unindent();
                }

                this.Indent();
                this.Generate(differences.Triggers, "Triggers");
                this.Unindent();

                this.Indent();
                this.Generate(differences.UniqueConstraints, "Unique constraints");
                this.Unindent();

                this.WriteProperties(differences.Properties);
            }
        }

        public void Visit(SqlUniqueConstraintDifferences differences)
        {
            this.WriteProperties(differences.Properties);

            this.Generate(differences.Columns, "Columns");
        }

        private void Generate<TSqlObject>(IEnumerable<SqlDatabaseObjectDifferences<TSqlObject>> differences, string typeName)
            where TSqlObject : SqlObject
        {
            if (!differences.Any())
            {
                return;
            }

            this.WriteLine($"------ {typeName} ------");

            foreach (var difference in differences)
            {
                this.WriteObjectName(difference);

                difference.Accept(this);
            }

            this.WriteLine();
        }

        private void WriteProperties(IEnumerable<SqlObjectPropertyDifference> properties)
        {
            this.Indent();

            foreach (var property in properties)
            {
                this.WriteProperty(property);
            }

            this.Unindent();
        }

        private void WriteProperty(SqlObjectPropertyDifference property)
        {
            this.WriteLine($"* {property.Name}:");

            this.Indent();
            this.Indent();

            this.WritePropertyValue("Source", property.Source);
            this.WritePropertyValue("Target", property.Target);

            this.Unindent();
            this.Unindent();
        }

        private void WritePropertyValue(string side, object? value)
        {
            this.Write($"{side}: ");

            if (value is string stringValue)
            {
                if (stringValue.Contains(Environment.NewLine))
                {
                    this.WriteLine();
                    this.Indent();
                    this.WriteLine(stringValue);
                    this.Unindent();
                    return;
                }
            }

            this.WriteLine(StringHelper.ToStringNull(value, "<No value>"));
        }

        private void WriteObjectName<TSqlObject>(SqlDatabaseObjectDifferences<TSqlObject> difference)
            where TSqlObject : SqlObject
        {
            this.Write("- ");

            if (difference.Source is null)
            {
                this.WriteLine($"{difference.Target} (Missing in the source)");
            }
            else if (difference.Target is null)
            {
                this.WriteLine($"{difference.Source} (Missing in the target)");
            }
            else
            {
                this.WriteLine($"{difference.Source}");
            }
        }

        private void Write(string value)
        {
            this.WriteIndent();
            this.writer.Write(value);
        }

        private void WriteLine()
        {
            this.WriteLine(string.Empty);
        }

        private void WriteLine(string value)
        {
            var lines = value.Split(Environment.NewLine);

            if (lines.Length > 1)
            {
                foreach (var line in lines)
                {
                    this.WriteLine(line);
                }

                return;
            }

            this.WriteIndent();

            value = value.Replace("\t", "  ");

            this.writer.WriteLine(value);

            this.writeTab = true;
        }

        private void Indent()
        {
            this.tab += 2;
        }

        private void Unindent()
        {
            this.tab -= 2;
        }

        private void WriteIndent()
        {
            if (this.writeTab)
            {
                this.writeTab = false;

                for (var i = 0; i < this.tab; i++)
                {
                    this.writer.Write(' ');
                }
            }
        }
    }
}
