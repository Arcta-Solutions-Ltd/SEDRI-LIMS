using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Security
{
    public interface IPreferenceHandler
    {
        Task<string> GetAsync(QueryFilterConfig queryFilters, TokenInfoModel token);
    }
}
