using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    public class ChildExistsClause : IWhereClause
    {
        private readonly TokenInfoModel _token;

        internal ChildExistsClause(TokenInfoModel token)
        {
            _token = token;
        }

        public string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias)
        {
            var subQuery = clause.SubQuery.BuildQuery(_token, clause.Filters);
            return $"{alias}.{clause.FieldToMatch ?? clause.Field} IN ( {subQuery} )";
        }
    }
}
