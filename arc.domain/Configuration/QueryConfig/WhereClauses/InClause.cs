using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class InClause : IWhereClause
    {
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var sanitizedIdList = SqlSanitizer.SanitizeIdList(value.First().Value);
            return " s." + (clause.FieldToMatch ?? clause.Field) + " in (" + sanitizedIdList + ") ";
        }
    }
}
