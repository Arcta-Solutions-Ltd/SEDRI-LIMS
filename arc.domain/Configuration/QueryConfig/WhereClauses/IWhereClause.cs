using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    public interface IWhereClause
    {
        string Get(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias);
    }
}
