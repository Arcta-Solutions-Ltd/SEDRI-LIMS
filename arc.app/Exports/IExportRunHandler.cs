using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public interface IExportRunHandler
    {
        Task<string> RunExportAsync(QueryFilterConfig queryFilters, TokenInfoModel token);
    }
}
