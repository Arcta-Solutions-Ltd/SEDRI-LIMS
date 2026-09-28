using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class EqualsClause : IWhereClause
    {
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var sanitizedValue = SqlSanitizer.SanitizeValue(value.First().Value);
            return $" {alias}.{clause.FieldToMatch ?? clause.Field} = '{sanitizedValue}' ";
        }
    }
}
