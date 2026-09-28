using arc.common.Models.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.SystemConfig
{
    public interface IJsonConfigRepository
    {
        Task<IEnumerable<ConfigsModel>> GetFormsForEventAsync(QueryFilterConfig queryFilters);
    }
}
