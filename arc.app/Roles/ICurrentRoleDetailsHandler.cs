using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Roles
{
    public interface ICurrentRoleDetailsHandler
    {
        Task<string> HandleAsync(QueryFilterConfig queryFilters);
    }
}
