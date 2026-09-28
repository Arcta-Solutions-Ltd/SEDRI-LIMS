using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class WithinLastClause : IWhereClause
    {
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var sanitizedValue = SqlSanitizer.SanitizeIntervalValue(value.First().Value);
            var sanitizedUnits = SqlSanitizer.SanitizeValue(clause.Units ?? "", 20);
            return $" s.{clause.FieldToMatch ?? clause.Field} > (now() - interval '{sanitizedValue} {sanitizedUnits}')";
        }
    }
}


// where lastmodifieddate > (now() - interval '15 days')
