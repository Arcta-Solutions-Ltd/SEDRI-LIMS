using arc.common;
using arc.common.Models;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app
{
    public interface IGenericRepository
    {
        void AddConfiguration(string tableName);
        void AddToken(TokenInfoModel token);
        Task<int> AddAsync(string dataToSave, EventModel command = null, string stringFields = "");
        Task<int> AddWithoutScopeAsync(string dataToSave, EventModel command = null, string stringFields = "");
        Task<string> GetFirstValueAsync();
        Task<string> GetSingleValueAsync(string field, string value);
        Task<string> GetSingleAsync(QueryConfig queryConfig, QueryFilterConfig filters = null);
        Task EditAsync(string dataToSave, string Id, EventModel command = null, string stringFields = "");
        Task<string> GetListAsync(QueryConfig queryConfig, QueryFilterConfig filters = null);
        long GetCountAsync(QueryConfig queryConfig, QueryFilterConfig filters = null, bool excludeTokenFilters = false);
        Task DeleteAsync(string id, EventModel command);
    }
}
