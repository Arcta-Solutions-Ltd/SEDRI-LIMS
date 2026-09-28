using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class OneOfClause : IWhereClause
    {
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var newWhere = "";
            string[] valuesToInclude = value.First().Value.ToLower().Split(",");
            if (valuesToInclude.Length > 0)
            {
                var sanitizedFirst = SqlSanitizer.SanitizeNumericValue(valuesToInclude[0].Trim());
                newWhere = $" ({alias}.{clause.FieldToMatch ?? clause.Field} = {sanitizedFirst}";
            }
            if (valuesToInclude.Length > 1)
            {
                for (var index = 1; index < valuesToInclude.Length; index++)
                {
                    var sanitizedValue = SqlSanitizer.SanitizeNumericValue(valuesToInclude[index].Trim());
                    newWhere += $" or {alias}.{clause.FieldToMatch ?? clause.Field} = {sanitizedValue}";
                }
            }
            return newWhere += ")";
        }
    }
}
