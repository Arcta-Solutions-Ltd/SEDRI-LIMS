using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class MultipleOneOfClause : IWhereClause
    {
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var newWhere = "";
            string[] valuesToInclude = value.First().Value.ToLower().Split(",");
            if (valuesToInclude.Length > 0)
            {
                var sanitizedFirst = SqlSanitizer.SanitizeSearchText(valuesToInclude[0].Trim());
                newWhere = $" (LOWER(s.{clause.FieldToMatch ?? clause.Field}) like '%{sanitizedFirst}%' ";
            }
            if (valuesToInclude.Length > 1)
            {
                for (var index = 1; index < valuesToInclude.Length; index++)
                {
                    var sanitizedValue = SqlSanitizer.SanitizeSearchText(valuesToInclude[index].Trim());
                    newWhere += $" or LOWER(s.{clause.FieldToMatch ?? clause.Field}) like '%{sanitizedValue}%' ";
                }
            }
            return newWhere += ")";
        }
    }
}
