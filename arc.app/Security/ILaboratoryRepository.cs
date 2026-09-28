using arc.common.Models;
using arc.common.Models.Laboratory;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Security
{
    public interface ILaboratoryRepository
    {
        Task<IEnumerable<OptionsConfig>> GetLaboratoriesForListAsync(TokenInfoModel token);
        Task<IEnumerable<OptionsConfig>> GetLaboratoriesForListUnfilteredAsync();
        Task<IEnumerable<LaboratoryListModel>> GetLaboratoryListAsync(QueryFilterConfig queryFilters);
        Task<LaboratoryListModel> LaboratoryByIdAsync(QueryFilterConfig queryFilters);
        Task DeleteLaboratoryAsync(string id);
    }
}
