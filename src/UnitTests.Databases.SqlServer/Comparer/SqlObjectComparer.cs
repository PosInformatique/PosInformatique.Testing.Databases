//-----------------------------------------------------------------------
// <copyright file="SqlObjectComparer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.UnitTests.Databases.SqlServer
{
    internal sealed class SqlObjectComparer : ISqlObjectVisitor<SqlObjectDifferences?>
    {
        private readonly SqlObject source;

        public SqlObjectComparer(SqlObject source)
        {
            this.source = source;
        }

        public static IList<SqlObjectDifferences<TSqlObject>> Compare<TSqlObject>(IReadOnlyList<TSqlObject> source, IReadOnlyList<TSqlObject> target, Func<TSqlObject, object> keySelector)
            where TSqlObject : SqlObject
        {
            var differences = new List<SqlObjectDifferences<TSqlObject>>();

            // Iterate on the target objects
            foreach (var targetObject in target)
            {
                var keyValue = keySelector(targetObject);
                var sourceObject = Find(source, keySelector, keyValue);

                if (sourceObject is null)
                {
                    // Missing in the source.
                    differences.Add(new SqlObjectDifferences<TSqlObject>(null, targetObject, SqlObjectDifferenceType.MissingInSource, null));
                }
                else
                {
                    // Compare the object using visitor pattern.
                    var difference = Compare(sourceObject, targetObject);

                    if (difference is not null)
                    {
                        differences.Add(difference);
                    }
                }
            }

            // Iterate on the source objects
            foreach (var sourceObject in source)
            {
                var keyValue = keySelector(sourceObject);
                var targetObject = Find(target, keySelector, keyValue);

                if (targetObject is null)
                {
                    // Missing in the target.
                    differences.Add(new SqlObjectDifferences<TSqlObject>(sourceObject, null, SqlObjectDifferenceType.MissingInTarget, null));
                }
            }

            return differences;
        }

        public static IList<SqlTableDifferences> Compare(IReadOnlyList<SqlTable> source, IReadOnlyList<SqlTable> target)
        {
            return Compare(source, target, t => t.Name, diff => new SqlTableDifferences(diff) { PrimaryKey = null });
        }

        public SqlObjectDifferences? Visit(SqlCheckConstraint checkConstraint)
        {
            return this.CreateDifferences(
                checkConstraint,
                this.CompareProperty(checkConstraint, ck => TsqlCodeHelper.RemoveNotUsefulCharacters(ck.Code), nameof(checkConstraint.Code), ck => ck.Code));
        }

        public SqlObjectDifferences? Visit(SqlColumn column)
        {
            return this.CreateDifferences(
                column,
                this.CompareProperty(column, t => t.Position, nameof(column.Position)),
                this.CompareProperty(column, t => t.SystemTypeId, nameof(column.SystemTypeId)),
                this.CompareProperty(column, t => t.MaxLength, nameof(column.MaxLength)),
                this.CompareProperty(column, t => t.Precision, nameof(column.Precision)),
                this.CompareProperty(column, t => t.Scale, nameof(column.Scale)),
                this.CompareProperty(column, t => t.IsNullable, nameof(column.IsNullable)),
                this.CompareProperty(column, t => t.IsIdentity, nameof(column.IsIdentity)),
                this.CompareProperty(column, t => t.CollationName, nameof(column.CollationName)),
                this.CompareProperty(column, t => t.IsComputed, nameof(column.IsComputed)),
                this.CompareProperty(column, t => TsqlCodeHelper.RemoveNotUsefulCharacters(t.ComputedExpression), nameof(column.ComputedExpression), t => t.ComputedExpression));
        }

        public SqlObjectDifferences? Visit(SqlForeignKey foreignKey)
        {
            var sourceForeignKey = (SqlForeignKey)this.source;

            // Compare the columns
            var columnsDifferences = Compare(sourceForeignKey.Columns, foreignKey.Columns, c => c.Name);

            // Compare the properties of the foreign key
            var differenceProperties = GetPropertyDifferences(
                this.CompareProperty(foreignKey, fk => fk.DeleteAction, nameof(foreignKey.DeleteAction)),
                this.CompareProperty(foreignKey, fk => fk.ReferencedTable, nameof(foreignKey.ReferencedTable)),
                this.CompareProperty(foreignKey, fk => fk.UpdateAction, nameof(foreignKey.UpdateAction)));

            if (columnsDifferences.Count + differenceProperties.Count > 0)
            {
                return new SqlForeignKeyDifferences(sourceForeignKey, foreignKey, SqlObjectDifferenceType.Different, differenceProperties, columnsDifferences);
            }

            return null;
        }

        public SqlObjectDifferences? Visit(SqlForeignKeyColumn column)
        {
            return this.CreateDifferences(
                column,
                this.CompareProperty(column, c => c.Name, nameof(column.Name)),
                this.CompareProperty(column, c => c.Position, nameof(column.Position)),
                this.CompareProperty(column, c => c.Referenced, nameof(column.Referenced)));
        }

        public SqlObjectDifferences? Visit(SqlIndex index)
        {
            var sourceIndex = (SqlIndex)this.source;

            // Compare the columns
            var columnsDifferences = Compare(sourceIndex.Columns, index.Columns, c => c.Name);

            // Compare the included columns
            var includedColumnsDifferences = Compare(sourceIndex.IncludedColumns, index.IncludedColumns, c => c.Name);

            // Compare the properties of the index
            var differenceProperties = GetPropertyDifferences(
                this.CompareProperty(index, i => i.IsUnique, nameof(index.IsUnique)),
                this.CompareProperty(index, i => TsqlCodeHelper.RemoveNotUsefulCharacters(i.Filter), nameof(index.Filter), i => i.Filter),
                this.CompareProperty(index, i => i.Type, nameof(index.Type)));

            if (columnsDifferences.Count + includedColumnsDifferences.Count + differenceProperties.Count > 0)
            {
                return new SqlIndexDifferences(sourceIndex, index, SqlObjectDifferenceType.Different, differenceProperties, columnsDifferences, includedColumnsDifferences);
            }

            return null;
        }

        public SqlObjectDifferences? Visit(SqlIndexColumn column)
        {
            return this.CreateDifferences(
                column,
                this.CompareProperty(column, c => c.Name, nameof(column.Name)),
                this.CompareProperty(column, c => c.Position, nameof(column.Position)));
        }

        public SqlObjectDifferences? Visit(SqlPrimaryKey primaryKey)
        {
            var sourcePrimaryKey = (SqlPrimaryKey)this.source;

            // Compare the columns
            var columnsDifferences = Compare(sourcePrimaryKey.Columns, primaryKey.Columns, c => c.Name);

            // Compare the properties of the primary key
            var differenceProperties = GetPropertyDifferences(
                this.CompareProperty(primaryKey, pk => pk.Name, nameof(primaryKey.Name)),
                this.CompareProperty(primaryKey, pk => pk.Type, nameof(primaryKey.Type)));

            if (columnsDifferences.Count + differenceProperties.Count > 0)
            {
                return new SqlPrimaryKeyDifferences(sourcePrimaryKey, primaryKey, SqlObjectDifferenceType.Different, differenceProperties, columnsDifferences);
            }

            return null;
        }

        public SqlObjectDifferences? Visit(SqlPrimaryKeyColumn column)
        {
            return this.CreateDifferences(
                column,
                this.CompareProperty(column, c => c.Name, nameof(column.Name)),
                this.CompareProperty(column, c => c.Position, nameof(column.Position)));
        }

        public SqlObjectDifferences? Visit(SqlStoredProcedure storedProcedure)
        {
            return this.CreateDifferences(
                storedProcedure,
                this.CompareProperty(storedProcedure, sp => TsqlCodeHelper.RemoveNotUsefulCharacters(sp.Code), nameof(storedProcedure.Code), sp => sp.Code));
        }

        public SqlObjectDifferences? Visit(SqlTable table)
        {
            var sourceTable = (SqlTable)this.source;

            // Compare the check constraints
            var checkConstraintDifferences = Compare(sourceTable.CheckConstraints, table.CheckConstraints, tr => tr.Name);

            // Compare the columns
            var columnsDifferences = Compare(sourceTable.Columns, table.Columns, c => c.Name);

            // Compare the foreign keys
            var foreignKeysDifferences = Compare(sourceTable.ForeignKeys, table.ForeignKeys, fk => fk.Name, diff => new SqlForeignKeyDifferences(diff));

            // Compare the indexes
            var indexesDifferences = Compare(sourceTable.Indexes, table.Indexes, i => i.Name, diff => new SqlIndexDifferences(diff));

            // Compare the primary key
            var primaryKeyDifferences = (SqlPrimaryKeyDifferences?)Compare(CreateArray(sourceTable.PrimaryKey), CreateArray(table.PrimaryKey), pk => pk.Name).SingleOrDefault();

            // Compare the triggers
            var triggersDifferences = Compare(sourceTable.Triggers, table.Triggers, tr => tr.Name);

            // Compare the unique constraints
            var uniqueConstraintsDifferences = Compare(sourceTable.UniqueConstraints, table.UniqueConstraints, uc => uc.Name, diff => new SqlUniqueConstraintDifferences(diff));

            if (columnsDifferences.Count + triggersDifferences.Count + checkConstraintDifferences.Count + indexesDifferences.Count + foreignKeysDifferences.Count + uniqueConstraintsDifferences.Count > 0 || primaryKeyDifferences is not null)
            {
                return new SqlTableDifferences(sourceTable, table, SqlObjectDifferenceType.Different, Array.Empty<SqlObjectPropertyDifference>(), columnsDifferences, triggersDifferences, checkConstraintDifferences, indexesDifferences, foreignKeysDifferences, uniqueConstraintsDifferences)
                {
                    PrimaryKey = primaryKeyDifferences,
                };
            }

            return this.CreateDifferences(table);
        }

        public SqlObjectDifferences? Visit(SqlTrigger trigger)
        {
            return this.CreateDifferences(
                trigger,
                this.CompareProperty(trigger, tr => tr.IsInsteadOfTrigger, nameof(trigger.IsInsteadOfTrigger)),
                this.CompareProperty(trigger, tr => TsqlCodeHelper.RemoveNotUsefulCharacters(tr.Code), nameof(trigger.Code), tr => tr.Code));
        }

        public SqlObjectDifferences? Visit(SqlUniqueConstraint uniqueConstraint)
        {
            var sourceUniqueConstraint = (SqlUniqueConstraint)this.source;

            // Compare the columns
            var columnsDifferences = Compare(sourceUniqueConstraint.Columns, uniqueConstraint.Columns, c => c.Name);

            // Compare the properties of the index
            var differenceProperties = GetPropertyDifferences(
                this.CompareProperty(uniqueConstraint, uc => uc.Type, nameof(uniqueConstraint.Type)));

            if (columnsDifferences.Count + differenceProperties.Count > 0)
            {
                return new SqlUniqueConstraintDifferences(sourceUniqueConstraint, uniqueConstraint, SqlObjectDifferenceType.Different, differenceProperties, columnsDifferences);
            }

            return null;
        }

        public SqlObjectDifferences? Visit(SqlUserType userType)
        {
            return this.CreateDifferences(
                userType,
                this.CompareProperty(userType, ut => ut.IsNullable, nameof(userType.IsNullable)),
                this.CompareProperty(userType, ut => ut.IsTableType, nameof(userType.IsTableType)),
                this.CompareProperty(userType, ut => ut.MaxLength, nameof(userType.MaxLength)),
                this.CompareProperty(userType, ut => ut.SystemTypeId, nameof(userType.SystemTypeId)));
        }

        public SqlObjectDifferences? Visit(SqlView view)
        {
            return this.CreateDifferences(
                view,
                this.CompareProperty(view, v => TsqlCodeHelper.RemoveNotUsefulCharacters(v.Code), nameof(view.Code), v => v.Code));
        }

        private static SqlObjectDifferences<TSqlObject>? Compare<TSqlObject>(TSqlObject source, TSqlObject target)
            where TSqlObject : SqlObject
        {
            var visitor = new SqlObjectComparer(source);

            return (SqlObjectDifferences<TSqlObject>?)target.Accept(visitor);
        }

        private static IList<TDifferences> Compare<TDifferences, TSqlObject>(IReadOnlyList<TSqlObject> source, IReadOnlyList<TSqlObject> target, Func<TSqlObject, object> keySelector, Func<SqlObjectDifferences<TSqlObject>, TDifferences> factory)
            where TSqlObject : SqlObject
            where TDifferences : SqlObjectDifferences<TSqlObject>
        {
            var differences = Compare(source, target, keySelector);

            var typedDifferences = new List<TDifferences>(differences.Count);

            foreach (var difference in differences)
            {
                if (difference is not TDifferences)
                {
                    typedDifferences.Add(factory(difference));
                }
                else
                {
                    typedDifferences.Add((TDifferences)difference);
                }
            }

            return typedDifferences;
        }

        private static IReadOnlyList<SqlObjectPropertyDifference> GetPropertyDifferences(params SqlObjectPropertyDifference?[] differences)
        {
            return differences.Where(d => d is not null).ToArray()!;
        }

        private static TSqlObject? Find<TSqlObject, TKey>(IReadOnlyList<TSqlObject> objects, Func<TSqlObject, TKey> keySelector, TKey value)
        {
            return objects.SingleOrDefault(o => Equals(keySelector(o), value));
        }

        private static T[] CreateArray<T>(T? value)
            where T : class
        {
            if (value is null)
            {
                return Array.Empty<T>();
            }

            return [value];
        }

        private SqlObjectPropertyDifference? CompareProperty<TSqlObject>(TSqlObject target, Func<TSqlObject, object?> propertyValueForComparison, string name, Func<TSqlObject, object?>? propertyValueToDisplay = null)
            where TSqlObject : SqlObject
        {
            var source = (TSqlObject)this.source;

            if (propertyValueToDisplay is null)
            {
                propertyValueToDisplay = propertyValueForComparison;
            }

            var sourceValue = propertyValueForComparison(source);
            var targetValue = propertyValueForComparison(target);

            if (!Equals(sourceValue, targetValue))
            {
                return new SqlObjectPropertyDifference(name, propertyValueToDisplay(source), propertyValueToDisplay(target));
            }

            return null;
        }

        private SqlObjectDifferences? CreateDifferences<TSqlObject>(TSqlObject target, params SqlObjectPropertyDifference?[] differences)
            where TSqlObject : SqlObject
        {
            var properties = differences.Where(d => d is not null).ToArray();

            if (properties.Length == 0)
            {
                return null;
            }

            return new SqlObjectDifferences<TSqlObject>((TSqlObject)this.source, target, SqlObjectDifferenceType.Different, properties!);
        }
    }
}
