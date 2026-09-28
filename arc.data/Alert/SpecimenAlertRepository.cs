using arc.app.Alert;
using arc.app.Common;
using arc.common.Models.Alert;
using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    public class SpecimenAlertRepository : ISpecimenAlertRepository
    {
        private readonly ISqlQuery _sqlQuery;
        private readonly ISqlCommand _sqlCommand;
        private readonly ILogWriter _logWriter;

        public SpecimenAlertRepository(ISqlQuery sqlQuery, ISqlCommand sqlCommand, ILogWriter logWriter)
        {
            _sqlQuery = sqlQuery;
            _sqlCommand = sqlCommand;
            _logWriter = logWriter;
        }

        public async Task<List<SpecimenAlertListModel>> SpecimenAlertListQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run specimen alert list query", "SpecimenAlertRepository", "SpecimenAlertListQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new SpecimenAlertListQuery(), "Specimen Alert List Query", queryFilters);
        }

        public async Task<List<AlertDetailsModel>> DirectTestAlertsQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run direct test alerts query", "SpecimenAlertRepository", "DirectTestAlertsQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new DirectTestAlertsQuery(), "Direct Test Alert Query", queryFilters);
        }

        public async Task<List<AlertDetailsModel>> OnlyOrganismExistsAlertQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run only organism exists alert query", "SpecimenAlertRepository", "OnlyOrganismExistsAlertQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OnlyOrganismExistsAlertQuery(), "Only Organism Exists Alert Query", queryFilters);
        }

        public async Task<List<AlertDetailsModel>> OrganismTestASTTestAlertQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run organism, test and ast alert query", "SpecimenAlertRepository", "OrganismTestASTTestAlertQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrganismTestASTTestAlertQuery(), "Organism + Tests + AST Tests Alert Query", queryFilters);
        }

        public async Task<List<AlertDetailsModel>> OrganismTestsAlertQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run organism and tests alert query", "SpecimenAlertRepository", "OrganismTestsAlertQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrganismTestsAlertQuery(), "Organism + Tests Alert Query", queryFilters);
        }

        public async Task<List<AlertDetailsModel>> StandardSpecimenAlertQueryAsync()
        {
            _logWriter.LogInfo("Run standard specimen alert query", "SpecimenAlertRepository", "StandardSpecimenAlertQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new StandardSpecimenAlertQuery(), "Organism + Tests Alert Query", new QueryFilterConfig());
        }

        public async Task<List<AlertDetailsModel>> OrganismASTTestAlertQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run organism and ast alert query", "SpecimenAlertRepository", "OrganismASTTestAlertQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrganismASTAlertQuery(), "Organism + AST Tests Alert Query", queryFilters);
        }

        public async Task<int> SaveSpecimenAlertAsync(SpecimenAlertCommandModel dataToSave)
        {
            _logWriter.LogInfo("Run save specimen alert command", "SpecimenAlertRepository", "SaveSpecimenAlertAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new SaveSpecimenAlertCommand(), "Save Specimen Alerts", dataToSave, _logWriter);
        }

        public async Task<List<AlertMessageModel>> SpecimenMessageListQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run specimen message list query", "SpecimenAlertRepository", "SpecimenMessageListQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new AlertListforSpecimenQuery(), "Specimen message list Query", queryFilters);
        }

        public async Task<List<AlertMessageModel>> CultureMessageListQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run culture message list query", "SpecimenAlertRepository", "CultureMessageListQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new AlertListForCultureQuery(), "Culture message list Query", queryFilters);
        }
    }
}
