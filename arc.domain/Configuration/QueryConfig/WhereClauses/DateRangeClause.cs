using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    /// <summary>
    /// Filters by a date field falling within a range using startdate and enddate parameters.
    /// Used for Patient list DateOfBirth filter.
    /// </summary>
    internal class DateRangeClause : IWhereClause
    {
        /// <summary>
        /// Produces SQL filtering the date field by startdate and enddate parameters.
        /// </summary>
        /// <param name="clause">The where clause config (FieldToMatch is the date column, e.g. dateofbirth).</param>
        /// <param name="value">Query parameters containing startdate and enddate.</param>
        /// <param name="alias">Table alias (e.g. "s").</param>
        /// <returns>SQL fragment for the date range filter.</returns>
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var parameters = value.ToList();
            var getVal = (string key) =>
            {
                var p = parameters.FirstOrDefault(v => v.Key.Equals(key, System.StringComparison.OrdinalIgnoreCase));
                return p != null && !string.IsNullOrEmpty(p.Value) ? p.Value.Trim() : null;
            };

            var startDate = getVal("startdate");
            var endDate = getVal("enddate");

            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
                return " (1=1) ";

            var dateCol = string.IsNullOrEmpty(alias)
                ? (clause.FieldToMatch ?? "dateofbirth")
                : alias + "." + (clause.FieldToMatch ?? "dateofbirth");

            var startSanitized = !string.IsNullOrEmpty(startDate) ? "'" + SqlSanitizer.SanitizeValue(startDate) + "'" : null;
            var endSanitized = !string.IsNullOrEmpty(endDate) ? "'" + SqlSanitizer.SanitizeValue(endDate) + "'" : null;

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                return $" ({dateCol}::date >= {startSanitized}::date and {dateCol}::date <= {endSanitized}::date) ";
            if (!string.IsNullOrEmpty(startDate))
                return $" ({dateCol}::date >= {startSanitized}::date) ";
            return $" ({dateCol}::date <= {endSanitized}::date) ";
        }
    }
}
