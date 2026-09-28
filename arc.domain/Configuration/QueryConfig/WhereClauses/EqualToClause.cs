using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class EqualToClause : IWhereClause
    {
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var sanitizedValue = SqlSanitizer.SanitizeNumericValue(value.First().Value);
            return $" s.{clause.FieldToMatch ?? clause.Field} {clause.Comparison} {sanitizedValue} ";
        }
    }
}
