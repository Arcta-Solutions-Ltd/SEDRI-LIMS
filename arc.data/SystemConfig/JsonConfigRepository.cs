using arc.app.SystemConfig;
using arc.common.Models.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    public class JsonConfigRepository : IJsonConfigRepository
    {
        private readonly ISqlQuery _sqlQuery;

        public JsonConfigRepository(ISqlQuery sqlQuery)
        {
            _sqlQuery = sqlQuery;
        }

        public async Task<IEnumerable<ConfigsModel>> GetFormsForEventAsync(QueryFilterConfig queryFilters)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new GetFormsForEventQuery(), "Get forms for event query", queryFilters);
        }
    }
}
