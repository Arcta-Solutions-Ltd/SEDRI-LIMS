using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Specimen
{
    public interface ICultureHandler
    {
        Task<string> GetCultureByIdAsync(QueryFilterConfig queryFilters);
    }
}
