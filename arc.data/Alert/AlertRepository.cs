using arc.app.Alert;
using arc.app.Common;
using arc.common.Models.Alert;
using arc.domain.Alert;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    public class AlertRepository : IAlertRepository
    {
        private readonly ISqlQuery _sqlQuery;
        private readonly ISqlCommand _sqlCommand;
        private readonly ILogWriter _logWriter;

        public AlertRepository(ISqlQuery sqlQuery, ISqlCommand sqlCommand, ILogWriter logWriter)
        {
            _sqlQuery = sqlQuery;
            _sqlCommand = sqlCommand;
            _logWriter = logWriter;
        }

        public async Task<List<AlertListModel>> AlertListQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run alert list query", "AlertRepository", "AlertListQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new AlertListQuery(), "Alert List Query", queryFilters);
        }

        public async Task<List<OrganismAlertListModel>> AlertsForOrganismListQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run alerts for organism list query", "AlertRepository", "AlertsForOrganismListQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new AlertsForOrganismListQuery(), "Organism Alert List Query", queryFilters);
        }

        public async Task<int> AddAlertAsync(AlertDetailsModel dataToSave)
        {
            _logWriter.LogInfo("Run add alert command", "AlertRepository", "AddAlertAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddAlertCommand(), "Add Alert", dataToSave);
        }

        public async Task<int> EditAlertAsync(AlertDetails dataToSave)
        {
            _logWriter.LogInfo("Run edit alert command", "AlertRepository", "EditAlertAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new EditAlertCommand(), "Edit Alert", dataToSave);
        }

        public async Task<AlertDetails> EditAlertQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run edit alert query", "AlertRepository", "EditAlertQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new EditAlertQuery(), "Edit Alert Query", queryFilters);
        }

        public async Task<AlertListModel> SingleAlertForAlertListQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run single alert for alert list query", "AlertRepository", "SingleAlertForAlertListQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new SingleAlertForAlertListQuery(), "Single Alert Query", queryFilters);
        }

        public async Task<AlertViewModel> AlertViewByIdQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run alert view query", "AlertRepository", "AlertViewByIdQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new AlertViewByIdQuery(), "Alert View Query", queryFilters);
        }

        public async Task<List<AlertSusceptibilityCriteriaModel>> AlertSusceptibilityCriteriaListByAlertIdQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run alert susceptibility criteria list query", "AlertRepository", "AlertSusceptibilityCriteriaListByAlertIdQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new AlertSusceptibilityCriteriaListByAlertIdQuery(), "Alert Susceptibility Criteria List", queryFilters);
        }

        public async Task<List<AlertTestCriteriaModel>> AlertTestCriteriaListByAlertIdQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run alert test criteria list query", "AlertRepository", "AlertTestCriteriaListByAlertIdQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new AlertTestCriteriaListByAlertIdQuery(), "Alert Test Criteria List", queryFilters);
        }

        public async Task DeleteAlertAsync(string id)
        {
            _logWriter.LogInfo("Run delete alert command", "AlertRepository", "DeleteAlertAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteAlertCommand(), "Delete Alert", id);
        }

        public async Task<List<OptionsConfig>> GetAlertCategoryListAsync()
        {
            _logWriter.LogInfo("Run get alert category list query", "AlertRepository", "GetAlertCategoryListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new AlertCategoryListQuery(), "Alert Category List Query", new QueryFilterConfig());
        }

        /// <summary>
        /// Adds a new alert approval or rejection record.
        /// </summary>
        /// <param name="data">The alert approval data containing AlertId, CodingStatusId, and RecordedBy.</param>
        /// <returns>The ID of the newly created alert approval record.</returns>
        public async Task<int> AddAlertApprovalAsync(AlertApproval data)
        {
            _logWriter.LogInfo("Run add alert approval command", "AlertRepository", "AddAlertApprovalAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddAlertApprovalCommand(), "Insert Alert Approval", data, _logWriter);
        }
    }
}
