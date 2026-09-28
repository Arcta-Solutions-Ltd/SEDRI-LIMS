using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    /// <summary>
    /// Filters patients by calculated age from DateOfBirth falling within a range.
    /// Uses AgeFromYears/Months/Days and AgeToYears/Months/Days parameters.
    /// Reference date is current date.
    /// </summary>
    internal class AgeRangeClause : IWhereClause
    {
        /// <summary>
        /// Produces SQL filtering dateofbirth such that age at current date is within the specified range.
        /// </summary>
        /// <param name="clause">The where clause config.</param>
        /// <param name="value">All query parameters (used to find age range params).</param>
        /// <param name="alias">Table alias for the Patient table (e.g. "s").</param>
        /// <returns>SQL fragment for the age range filter.</returns>
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var parameters = value.ToList();
            var getVal = (string key) =>
            {
                var p = parameters.FirstOrDefault(v => v.Key.Equals(key, System.StringComparison.OrdinalIgnoreCase));
                return p != null && !string.IsNullOrEmpty(p.Value) ? SqlSanitizer.SanitizeNumericValue(p.Value.Trim(), "") : "";
            };

            var fromY = getVal("AgeFromYears");
            var fromM = getVal("AgeFromMonths");
            var fromD = getVal("AgeFromDays");
            var toY = getVal("AgeToYears");
            var toM = getVal("AgeToMonths");
            var toD = getVal("AgeToDays");

            var hasFrom = !string.IsNullOrEmpty(fromY) || !string.IsNullOrEmpty(fromM) || !string.IsNullOrEmpty(fromD);
            var hasTo = !string.IsNullOrEmpty(toY) || !string.IsNullOrEmpty(toM) || !string.IsNullOrEmpty(toD);

            if (!hasFrom && !hasTo) return " (1=1) ";

            var dobCol = string.IsNullOrEmpty(alias) ? "dateofbirth" : alias + ".dateofbirth";
            var refDate = "current_date";

            var fy = string.IsNullOrEmpty(fromY) ? "0" : fromY;
            var fm = string.IsNullOrEmpty(fromM) ? "0" : fromM;
            var fd = string.IsNullOrEmpty(fromD) ? "0" : fromD;
            var ty = string.IsNullOrEmpty(toY) ? "0" : toY;
            var tm = string.IsNullOrEmpty(toM) ? "0" : toM;
            var td = string.IsNullOrEmpty(toD) ? "0" : toD;
            var fromInterval = $"make_interval(years => {fy}::int, months => {fm}::int, days => {fd}::int)";
            var toInterval = $"make_interval(years => {ty}::int, months => {tm}::int, days => {td}::int)";

            string sqlFragment;
            if (hasFrom && !hasTo)
                sqlFragment = $" ({dobCol} is not null and {dobCol} <= {refDate} - {fromInterval}) ";
            else if (!hasFrom && hasTo)
                sqlFragment = $" ({dobCol} is not null and {dobCol} >= {refDate} - {toInterval}) ";
            else
                sqlFragment = $" ({dobCol} is not null and {dobCol} >= {refDate} - {toInterval} and {dobCol} <= {refDate} - {fromInterval}) ";

            return sqlFragment;
        }
    }
}
