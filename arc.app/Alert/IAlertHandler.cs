using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Alert
{
    public interface IAlertHandler
    {
        Task RaiseAlertAsync(int specimenId);
        Task<string> GetAlertAsync(QueryFilterConfig queryFilters);
    }
}
