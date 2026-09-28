using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    /// <summary>
    /// Where clause that includes records where the field is null, zero, or exists in a subquery.
    /// Used for tables (e.g. Queue) where items without a linked parent (e.g. SpecimenId = 0 or NULL) should be visible alongside items that match the subquery.
    /// </summary>
    public class ChildExistsOrNullClause : IWhereClause
    {
        private readonly TokenInfoModel _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildExistsOrNullClause"/> class.
        /// </summary>
        /// <param name="token">The token information model for building the subquery.</param>
        internal ChildExistsOrNullClause(TokenInfoModel token)
        {
            _token = token;
        }

        /// <summary>
        /// Generates a WHERE clause: (field IS NULL OR field = 0 OR field IN (subquery)).
        /// </summary>
        /// <param name="clause">The query where field configuration containing the subquery.</param>
        /// <param name="value">The query values (unused; required by interface).</param>
        /// <param name="alias">The table alias for the field.</param>
        /// <returns>The SQL fragment for the where clause.</returns>
        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var field = clause.FieldToMatch ?? clause.Field;
            var qualifiedField = $"{alias}.{field}";
            var subQuery = clause.SubQuery.BuildQuery(_token, clause.Filters);
            return $"({qualifiedField} IS NULL OR {qualifiedField} = 0 OR {qualifiedField} IN ( {subQuery} ))";
        }
    }
}
