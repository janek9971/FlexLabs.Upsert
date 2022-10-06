using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlexLabs.EntityFrameworkCore.Upsert.Internal;

namespace FlexLabs.EntityFrameworkCore.Upsert.Runners
{
    /// <summary>
    /// Upsert command runner for the Microsoft.EntityFrameworkCore.SqlServer provider
    /// </summary>
    public class OracleUpsertCommandRunner : RelationalUpsertCommandRunner
    {
        private static readonly string[] Providers = { "Oracle.EntityFrameworkCore", "Devart.Data.Oracle.Entity.EFCore" };
        /// <inheritdoc/>
        public override bool Supports(string providerName)
        {
            return Providers.Any(provider=>provider == providerName);
        }

        /// <inheritdoc/>
        protected override string EscapeName(string name) => name;
        /// <inheritdoc/>
        protected override string? SourcePrefix => "S";
        /// <inheritdoc/>
        protected override string? TargetPrefix => "T";
        /// <inheritdoc/>
        protected override string Parameter(int index) => ":p" + index;

        /// <inheritdoc/>
        protected override int? MaxQueryParams => 2100;

        /// <inheritdoc/>
        public override string GenerateCommand(string tableName, ICollection<ICollection<(string ColumnName, ConstantValue Value, string DefaultSql, bool AllowInserts)>> entities,
            ICollection<(string ColumnName, bool IsNullable)> joinColumns, ICollection<(string ColumnName, IKnownValue Value)>? updateExpressions,
            KnownExpression? updateCondition)
        {
            var result = new StringBuilder();
            result.Append($"MERGE INTO {tableName} {TargetPrefix} USING ( SELECT ");
            result.Append(string.Join(", ", entities.Select(ec => string.Join(", ", ec.Select(e => $"{Parameter(e.Value.ArgumentIndex)} as {e.ColumnName}")))));
            result.Append($" FROM DUAL   ) {SourcePrefix} ON (");
            result.Append(string.Join(" AND ", joinColumns.Select(c => c.IsNullable
                ? $"(({SourcePrefix}.{c.ColumnName} IS NULL AND {TargetPrefix}.{c.ColumnName} IS NULL) OR ({SourcePrefix}.{c.ColumnName}]IS NOT NULL AND {TargetPrefix}.{c.ColumnName} = {SourcePrefix}.{c.ColumnName}))"
                : $"{TargetPrefix}.{c.ColumnName} = {SourcePrefix}.{c.ColumnName}")));
            result.Append(") WHEN NOT MATCHED THEN INSERT  (");
            result.Append(string.Join(", ", entities.First().Where(e => e.AllowInserts).Select(e => $"{TargetPrefix}." + EscapeName(e.ColumnName))));
            result.Append(") VALUES (");
            result.Append(string.Join(", ", entities.First().Where(e => e.AllowInserts).Select(e => $"{SourcePrefix}." + EscapeName(e.ColumnName))));
            result.Append(')');
            if (updateExpressions != null)
            {
                result.Append(" WHEN MATCHED");
                if (updateCondition != null)
                    result.Append($" AND {ExpandExpression(updateCondition)}");
                result.Append(" THEN UPDATE SET ");
                result.Append(string.Join(", ", updateExpressions.Select((e, i) => $"{EscapeName(e.ColumnName)} = {ExpandValue(e.Value)}")));
            }
            return result.ToString();
        }
    }
}
