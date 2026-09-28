using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public interface IExportRunRepository
    {
        public Task<ExportRunResponseModel> RunExportAsync(QueryFilterConfig queryFilterConfig, TokenInfoModel token);
    }
}
