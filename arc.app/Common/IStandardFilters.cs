using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IStandardFilters
    {
        Task<TokenInfoModel> UpdateTokenAsync(TokenInfoModel token);
        Task<QueryFilterConfig> UpdateFilterAsync(QueryFilterConfig queryFilters);
        //Task<QueryFilterConfig> ApplyTokenToQueryFiltersAsync(QueryFilterConfig queryFilters, TokenInfoModel token);
    }
}
