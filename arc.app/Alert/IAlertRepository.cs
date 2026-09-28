using arc.common.Models.Alert;
using arc.domain.Alert;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Alert
{
    public interface IAlertRepository
    {
        Task<List<AlertListModel>> AlertListQueryAsync(QueryFilterConfig queryFilters);
        Task<int> AddAlertAsync(AlertDetailsModel dataToSave);
        Task<int> EditAlertAsync(AlertDetails dataToSave);
        Task<AlertDetails> EditAlertQueryAsync(QueryFilterConfig queryFilters);
        Task DeleteAlertAsync(string id);
        Task<AlertListModel> SingleAlertForAlertListQueryAsync(QueryFilterConfig queryFilters);
        Task<AlertViewModel> AlertViewByIdQueryAsync(QueryFilterConfig queryFilters);
        Task<List<AlertSusceptibilityCriteriaModel>> AlertSusceptibilityCriteriaListByAlertIdQueryAsync(QueryFilterConfig queryFilters);
        Task<List<AlertTestCriteriaModel>> AlertTestCriteriaListByAlertIdQueryAsync(QueryFilterConfig queryFilters);
        Task<List<OrganismAlertListModel>> AlertsForOrganismListQueryAsync(QueryFilterConfig queryFilters);
        Task<List<OptionsConfig>> GetAlertCategoryListAsync();
        Task<int> AddAlertApprovalAsync(AlertApproval data);
    }
}
