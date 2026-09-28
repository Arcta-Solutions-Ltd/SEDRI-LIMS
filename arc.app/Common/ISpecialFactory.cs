using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface ISpecialFactory
    {
        Task<string> RunQueryAsync(string queryName, QueryFilterConfig parameters = null, TokenInfoModel token = null);
    }
}
