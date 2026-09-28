using arc.app.Common;
using arc.app.Quality;
using arc.common.Models.Quality;
using arc.common.Models.QualityAssurance;
using arc.data.Quality.Commands;
using arc.data.Quality.Queries;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Quality;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    public class QualityRepository : IQualityRepository
    {
        private readonly ISqlQuery _sqlQuery;
        private readonly ISqlCommand _sqlCommand;
        private readonly ILogWriter _logWriter;

        public QualityRepository(ISqlQuery sqlQuery, ISqlCommand sqlCommand, ILogWriter logWriter)
        {
            _sqlQuery = sqlQuery;
            _sqlCommand = sqlCommand;
            _logWriter = logWriter;
        }

        public async Task<List<IqcResultGridViewModel>> GetIqcTestResultsAsync(QueryFilterConfig queryFilterConfig)
        {
            _logWriter.LogInfo("Run get IQC test results query", nameof(QualityRepository), nameof(GetIqcTestResultsAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new GetIqcTestResultsQuery(), "Get IQC test results", queryFilterConfig);
        }

        public async Task<List<IqcTestProfileListModel>> GetIqcTestProfileListQueryAsync(QueryFilterConfig queryFilterConfig)
        {
            _logWriter.LogInfo("Run get IQC test profile list query", nameof(QualityRepository), nameof(GetIqcTestProfileListQueryAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new IqcTestProfileListQuery(), "Get IQC Test Profile List", queryFilterConfig);
        }

        public async Task<IqcTestProfileListModel> GetIqcTestProfileSingleQueryAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new IqcTestProfileSingleQuery(), "Get Single IQC test profile", queryFilterConfig);
        }

        public async Task<List<OrganismAntibioticModel>> GetQcAntibioticsForIqcTestProfileQcOrganismAsync(QueryFilterConfig queryFilterConfig)
        {
            _logWriter.LogInfo("Run get antibiotic list for IQC test profile organism query", nameof(QualityRepository), nameof(GetQcAntibioticsForIqcTestProfileQcOrganismAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new GetAntibioticsForIqcTestProfileQcOrganismQuery(), "Get antibiotic list for IQC test profile organism", queryFilterConfig);
        }

        public async Task<int> EditIqcTestProfileAsync(QualityCraftedModel data)
        {
            _logWriter.LogInfo("Run edit quality assurance profile command", nameof(QualityRepository), nameof(EditIqcTestProfileAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new EditIqcTestProfileCommand(), nameof(EditIqcTestProfileAsync), data);
        }

        public async Task<QcOrganism> GetQcOrganismByIdAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new GetQcOrganismByIdQuery(), "Get QcOrganism By Id", queryFilterConfig);
        }

        public async Task<int> AddIqcTestAsync(AddIqcTestModel addIqcTestModel)
        {
            var id = await _sqlCommand.CommandWithTypeQueryAsync(new AddIqcTestCommand(), "Add IQC Test command", addIqcTestModel);
            return id;
        }

        public async Task<int> EditIqcResultAsync(IqcTest iqcTest)
        {
            _logWriter.LogInfo("Run edit IQC test command", nameof(QualityRepository), nameof(EditIqcResultAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new EditIqcResultCommand(), "Edit IQC result command", iqcTest);
        }

        public async Task<int> RunIqcTestAsync(IqcTest iqcTest)
        {
            _logWriter.LogInfo("Run IQC test command", nameof(QualityRepository), nameof(RunIqcTestAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new RunIqcTestCommand(), "Run IQC test command", iqcTest);
        }

        public async Task<EditIqcTestModel> RunIqcTestQueryAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new GetIqcTestForRunIqcTestInitialQuery(), nameof(GetIqcTestForRunIqcTestInitialQuery), queryFilterConfig);
        }

        public async Task<List<IqcTestProfileQcOrganismsModel>> GetQcOrganismsForIqcTestAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new QcOrganismsForIqcTestQuery(), nameof(QcOrganismsForIqcTestQuery), queryFilterConfig);
        }

        public async Task<int> EditIqcTestQcOrganismsAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<IqcTestModel>(dataToSave);
            var id = await _sqlCommand.CommandWithTypeQueryAsync(new EditIqcTestQcOrganismsCommand(), nameof(EditIqcTestQcOrganismsCommand), data);
            return id;
        }

        public async Task DeleteIqcTestProfileAsync(string listItemId)
        {
            await _sqlCommand.CarryOutCommandAsync(new SoftDeleteIqcTestProfileCommand(), nameof(SoftDeleteIqcTestProfileCommand), listItemId);
        }

        public async Task<bool> IsIqcTestProfileOrganismUsedByDefaultAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new IsIqcTestProfileOrganismUsedByDefaultQuery(), nameof(IsIqcTestProfileOrganismUsedByDefaultQuery), queryFilterConfig);
        }

        public async Task<List<IqcTestProfileQcAntibioticTableRow>> GetMicAntibioticsDetailsForIqcTestProfileQcOrganismAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(
                new GetMicAntibioticsDetailsForIqcTestProfileQcOrganismQuery(), nameof(GetMicAntibioticsDetailsForIqcTestProfileQcOrganismQuery), queryFilterConfig);
        }

        public async Task<List<IqcTestProfileQcAntibioticTableRow>> GetDiskAntibioticsDetailsForIqcTestProfileQcOrganismAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(
                new GetDiskAntibioticsDetailsForIqcTestProfileQcOrganismQuery(), nameof(GetDiskAntibioticsDetailsForIqcTestProfileQcOrganismQuery), queryFilterConfig);
        }

        public async Task<int> AddIqcTestProfileAsync(IqcTestProfile iqcTestProfile)
        {
            return await _sqlCommand.CommandWithTypeQueryAsync(
                new AddIqcTestProfileCommand(), nameof(AddIqcTestProfileCommand), iqcTestProfile);
        }

        public async Task<List<QcOrganism>> GetAllQcOrganismsByTestMethodWithChildrenAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(
                new GetQcOrganismsByTestMethodQuery(), nameof(GetQcOrganismsByTestMethodQuery), queryFilterConfig);
        }

        public async Task<IqcTestProfile> GetIqcTestProfileByNameAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(
                new GetIqcTestProfileByNameQuery(), nameof(GetIqcTestProfileByNameQuery), queryFilterConfig);
        }

        public async Task<EditIqcTestModel> EditIqcResultQueryAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new GetIqcTestByResultIdQuery(), nameof(GetIqcTestByResultIdQuery), queryFilterConfig);
        }

        public async Task<IqcTest> GetIqcTestByIdQueryAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new IqcTestByIdQuery(), nameof(IqcTestByIdQuery), queryFilterConfig);
        }

        public async Task<IEnumerable<OptionsConfig>> GetIqcTestProfileNamesListAsync()
        {
            return await _sqlQuery.QueryReturningTypeAsync(new IqcTestProfileNamesListQuery(), nameof(IqcTestProfileNamesListQuery), new QueryFilterConfig());
        }

        public async Task<IqcTestProfile> GetIqcTestProfileByIdAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new GetIqcTestProfileByIdQuery(), nameof(GetIqcTestProfileByIdQuery), queryFilterConfig);
        }

        public async Task MarkIqcTestCompleteAsync(int iqcTestId, int completedListItemId)
        {
            await _sqlCommand.CarryOutCommandAsync(new MarkIqcTestCompleteCommand(), nameof(MarkIqcTestCompleteCommand), new[] { $"{iqcTestId}", $"{completedListItemId}" });
        }

        public async Task<List<IqcTestListModel>> GetIqcTestsListQueryAsync(QueryFilterConfig queryFilterConfig)
        {
            return await _sqlQuery.QueryReturningTypeAsync(new IqcTestsListQuery(), nameof(IqcTestsListQuery), queryFilterConfig);
        }

        public async Task<int> GetIqcTestIdFromIqcResultIdAsync(int iqcResultId)
        {
            var queryFilterConfig = new QueryFilterConfig();
            queryFilterConfig.AddInteger("iqcResultId", iqcResultId);
            return await _sqlQuery.QueryReturningTypeAsync(new GetIqcTestIdFromIqcResultIdQuery(), nameof(GetIqcTestIdFromIqcResultIdQuery), queryFilterConfig);
        }
    }
}
