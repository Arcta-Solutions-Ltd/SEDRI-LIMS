using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    public interface ICodingRepository
    {
        Task<int> GetBreakpointCountBySourceGuidelinesIdAsync(QueryFilterConfig queryFilters);
        Task<int> GetExpertRuleCountBySourceGuidelinesIdAsync(QueryFilterConfig queryFilters);
        Task DeleteCodingListAsync(string id);
        Task DeleteOrganismAsync(string id);
        Task<int> AddCustomEntryAsync(string name);
        Task EditCustomEntryAsync(string dataToSave);
        Task<int> GetCustomerEntryAsync(QueryFilterConfig queryFilters);
        Task<int> CheckCustomEntryCodeAsync(QueryFilterConfig queryFilters);
        Task<EditCustomEntryModel> GetCustomerEntryByOrganismIdAsync(QueryFilterConfig queryFilters);
    }
}
