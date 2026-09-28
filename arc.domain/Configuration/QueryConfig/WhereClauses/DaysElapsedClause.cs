using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class DaysElapsedClause : IWhereClause
    {
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var newWhere = "";
            var field = clause.FieldToMatch ?? clause.Field;
            var valuesToInclude = value.First().Value.ToLower().Split(",");
            if (valuesToInclude.Length > 0)
            {
                var sanitized = SqlSanitizer.SanitizeIntervalValue(valuesToInclude[0].Trim());
                if (sanitized.StartsWith(">"))
                {
                    var days = sanitized.Substring(1);
                    newWhere = $" (s.{field} < (DATE_TRUNC('day', NOW()) - interval '{days} days')";
                }
                else
                {
                    var days = int.Parse(sanitized);
                    newWhere = $" ((s.{field} >= (DATE_TRUNC('day', NOW()) - interval '{days} days') " +
                        $"and s.{field} < (DATE_TRUNC('day', NOW()) - interval '{days - 1} days'))";
                }
            }
            if (valuesToInclude.Length > 1)
            {
                for (var index = 1; index < valuesToInclude.Length; index++)
                {
                    var sanitized = SqlSanitizer.SanitizeIntervalValue(valuesToInclude[index].Trim());
                    if (sanitized.StartsWith(">"))
                    {
                        var days = sanitized.Substring(1);
                        newWhere += $" or (s.{field} < (DATE_TRUNC('day', NOW()) - interval '{days} days'))";
                    }
                    else
                    {
                        var days = int.Parse(sanitized);
                        newWhere += $" or (s.{field} >= (DATE_TRUNC('day', NOW()) - interval '{days} days') " +
                            $"and s.{field} < (DATE_TRUNC('day', NOW()) - interval '{days - 1} days'))";
                    }
                }
            }
            return newWhere += ")";
        }
    }
}
