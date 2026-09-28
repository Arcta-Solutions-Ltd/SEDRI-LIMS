using arc.common.Models.Coding;
using arc.domain.Coding;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    public interface IAntibioticRepository
    {
        Task<IEnumerable<OptionsConfig>> GetAntibioticListAsync();
        Task<IEnumerable<OptionsConfig>> GetAntibioticListWithGroupsAsync();
        Task<IEnumerable<OptionsConfig>> GetResistantAntibioticQueryAsync(QueryFilterConfig queryFilters);
        Task<List<AntibioticListModel>> GetAntibioticListForViewAsync(QueryFilterConfig queryFilters);
        Task DeleteAntibioticGroupAsync(string id);
        Task<AntibioticListModel> GetAntibioticEntryByIdAsync(QueryFilterConfig queryFilters);
        Task<Antibiotic> GetAntibioticEntryByCodingIdAsync(QueryFilterConfig queryFilters);
        Task<AntibioticListModel> GetAntibioticEntryByCodeAsync(QueryFilterConfig queryFilters);
        Task<int> AddAntibioticAsync(string dataToSave);
        Task<Dictionary<int, string>> GetAntibioticNamesByIdsAsync(IEnumerable<int> ids);
    }
}
