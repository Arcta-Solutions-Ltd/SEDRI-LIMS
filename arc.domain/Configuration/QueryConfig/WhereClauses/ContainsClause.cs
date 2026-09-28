using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class ContainsClause : IWhereClause
    {
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var sanitizedValue = SqlSanitizer.SanitizeSearchText(value.First().Value.ToLower());
            if (sanitizedValue != "")
            {
                return $" LOWER({alias}.{clause.FieldToMatch ?? clause.Field}) like '%{sanitizedValue}%' ";
            };
            return "";
        }
    }
}
