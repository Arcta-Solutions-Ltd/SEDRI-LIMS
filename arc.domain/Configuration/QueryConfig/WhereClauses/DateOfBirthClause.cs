using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class DateOfBirthClause : IWhereClause
    {
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var values = value.First().Value.Split(",");
            var minAge = SqlSanitizer.SanitizeNumericValue(values[0].Trim(), "");
            var maxAge = values.Length > 1 ? SqlSanitizer.SanitizeNumericValue(values[1].Trim(), "") : "";

            if (!string.IsNullOrEmpty(minAge) && string.IsNullOrEmpty(maxAge))
            {
                return " case when s.dateofbirth is null then s.age::int >= " + minAge + " else date_part('year', age(dateofbirth)) >= " + minAge + " end ";
            }
            if (string.IsNullOrEmpty(minAge) && !string.IsNullOrEmpty(maxAge))
            {
                return " case when s.dateofbirth is null then s.age::int <= " + maxAge + " else date_part('year', age(dateofbirth)) <= " + maxAge + " end ";
            }
            return " case when s.dateofbirth is null then s.age::int >= " + minAge + " and s.age::int <= " + maxAge + " else date_part('year', age(dateofbirth)) >= " + minAge + " and date_part('year',age(dateofbirth)) <= " + maxAge + " end ";
        }
    }
}
