using arc.common.Models.Alert;
using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Alert
{
    public interface ISpecimenAlertRepository
    {
        Task<List<SpecimenAlertListModel>> SpecimenAlertListQueryAsync(QueryFilterConfig queryFilters);
        Task<List<AlertDetailsModel>> DirectTestAlertsQueryAsync(QueryFilterConfig queryFilters);
        Task<int> SaveSpecimenAlertAsync(SpecimenAlertCommandModel dataToSave);
        Task<List<AlertMessageModel>> SpecimenMessageListQueryAsync(QueryFilterConfig queryFilters);
        Task<List<AlertDetailsModel>> OnlyOrganismExistsAlertQueryAsync(QueryFilterConfig queryFilters);
        Task<List<AlertDetailsModel>> OrganismTestASTTestAlertQueryAsync(QueryFilterConfig queryFilters);
        Task<List<AlertDetailsModel>> OrganismTestsAlertQueryAsync(QueryFilterConfig queryFilters);
        Task<List<AlertDetailsModel>> OrganismASTTestAlertQueryAsync(QueryFilterConfig queryFilters);
        Task<List<AlertMessageModel>> CultureMessageListQueryAsync(QueryFilterConfig queryFilters);
        Task<List<AlertDetailsModel>> StandardSpecimenAlertQueryAsync();
    }
}
