using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    public interface IIqcTestProfileQueryHandler
    {
        Task<string> GetProfileAsync(QueryFilterConfig queryFilter);
        Task<string> GetQcOrganismsProfileAsync(QueryFilterConfig queryFilter);
        Task<string> GetQcOrganismsForEditIqcTestQcOrganismsPageAsync(QueryFilterConfig queryFilter);
    }
}
