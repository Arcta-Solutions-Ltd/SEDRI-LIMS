using arc.common.Models;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Security
{
    public interface IOrganisationRepository
    {
        Task<int> AddAsync(string dataToSave);
        Task<int> EditAsync(string dataToSave);
        Task<IEnumerable<OptionsConfig>> GetOrganisationsForListAsync(TokenInfoModel token);
        Task<IEnumerable<OptionsConfig>> GetOrganisationsForListUnfilteredAsync();
        Task<OptionsConfig> GetOrganisationsForNameByIdAsync(int id);
        Task<int> GetOrganisationForValidationByIdAsync(QueryFilterConfig parameters);
        Task<string> GetOrganisationHierarchyAsync(string id);
        Task<bool> IsOrganisationEnabledAsync(int organisationId);

    }
}
