using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IQueryRun
    {
        Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null);
    }
}
