using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class StartsWithClause : IWhereClause
    {
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var sanitizedValue = SqlSanitizer.SanitizeSearchText(value.First().Value.ToLower());
            return $" LOWER(s.{clause.FieldToMatch ?? clause.Field}) like '{sanitizedValue}%' ";
        }
    }
}
