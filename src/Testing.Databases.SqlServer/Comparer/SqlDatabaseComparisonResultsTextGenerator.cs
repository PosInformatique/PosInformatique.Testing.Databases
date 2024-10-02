//-----------------------------------------------------------------------
// <copyright file="SqlDatabaseComparisonResultsTextGenerator.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    internal sealed class SqlDatabaseComparisonResultsTextGenerator : ISqlObjectDifferencesVisitor, IDisposable
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

        public static string Generate<TSqlObject>(SqlObjectDifferences<TSqlObject> differences)
            where TSqlObject : SqlObject
        {
            using var generator = new SqlDatabaseComparisonResultsTextGenerator();

            generator.GenerateCore(differences);

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

        public void Visit<TSqlObject>(SqlObjectDifferences<TSqlObject> differences)
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

        public void Visit(SqlTableDifferences differences)
        {
            if (differences.Source is not null && differences.Target is not null)
            {
                this.Generate(differences.CheckConstraints, "Check constraints");

                this.Generate(differences.Columns, "Columns");

                this.Generate(differences.ForeignKeys, "Foreign keys");

                this.Generate(differences.Indexes, "Indexes");

                if (differences.PrimaryKey is not null)
                {
                    this.Indent();
                    this.WriteLine($"------ Primary key ------");
                    differences.PrimaryKey.Accept(this);
                    this.Unindent();
                }

                this.Generate(differences.Triggers, "Triggers");

                this.Generate(differences.UniqueConstraints, "Unique constraints");

                this.WriteProperties(differences.Properties);
            }
        }

        public void Visit(SqlUniqueConstraintDifferences differences)
        {
            this.WriteProperties(differences.Properties);

            this.Generate(differences.Columns, "Columns");
        }

        private void Generate<TSqlObject>(IEnumerable<SqlObjectDifferences<TSqlObject>> differences, string typeName)
            where TSqlObject : SqlObject
        {
            if (!differences.Any())
            {
                return;
            }

            this.WriteLine($"------ {typeName} ------");

            foreach (var difference in differences)
            {
                this.Write("- ");
                this.GenerateCore(difference);
            }
        }

        private void GenerateCore<TSqlObject>(SqlObjectDifferences<TSqlObject> differences)
            where TSqlObject : SqlObject
        {
            this.WriteObjectName(differences);

            this.Indent();
            differences.Accept(this);
            this.Unindent();
        }

        private void WriteProperties(IEnumerable<SqlObjectPropertyDifference> properties)
        {
            foreach (var property in properties)
            {
                this.WriteProperty(property);
            }
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

        private void WriteObjectName<TSqlObject>(SqlObjectDifferences<TSqlObject> difference)
            where TSqlObject : SqlObject
        {
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
            var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

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
