using arc.app.Coding;
using arc.app.Common;
using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    public class CodingRepository : ICodingRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        public CodingRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
        }

        public async Task DeleteCodingListAsync(string id)
        {
            _logWriter.LogInfo("Run delete coding list command", "CodingRepository", "DeleteCodingListAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteCodingListCommand(), "Delete Coding List", id);
        }

        public async Task DeleteOrganismAsync(string id)
        {
            _logWriter.LogInfo("Run delete organism command", "CodingRepository", "DeleteOrganismAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteOrganismCommand(), "Delete Organism", id);
        }

        public async Task<int> AddCustomEntryAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<CustomEntryModel>(dataToSave);
            _logWriter.LogInfo("Run add custom entry command", "CodingRepository", "AddCustomEntryAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddCustomEntryCommand(), "Insert Custom Entry", data);
        }

        public async Task EditCustomEntryAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<CustomEntryModel>(dataToSave);
            _logWriter.LogInfo("Run edit custom entry command", "CodingRepository", "EditCustomEntryAsync");
            await _sqlCommand.CommandWithTypeQueryAsync(new EditCustomEntryCommand(), "Edit Custom Entry", data);
        }

        public async Task<int> GetCustomerEntryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get custom entry query", "CodingRepository", "GetCustomerEntryAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new GetCustomEntryQuery(), "Get Custom Entry", queryFilters);
        }

        public async Task<int> CheckCustomEntryCodeAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run check custom entry code query", "CodingRepository", "GetCustomEntryCodeAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new CheckCustomEntryCodeQuery(), "Get Custom Entry", queryFilters);
        }

        public async Task<EditCustomEntryModel> GetCustomerEntryByOrganismIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get custom entry by organismid query", "CodingRepository", "GetCustomerEntryByOrganismIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new CustomEntryByOrganismIdQuery(), "Get Custom Entry", queryFilters);
        }

        /// <summary>
        /// Counts breakpoints that reference specifications with the given guidelinesid (source/listitem id).
        /// Used when deleting a source to validate no breakpoints depend on it.
        /// </summary>
        public async Task<int> GetBreakpointCountBySourceGuidelinesIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run breakpoint count by source guidelines id query", "CodingRepository", "GetBreakpointCountBySourceGuidelinesIdAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new BreakpointCountBySourceGuidelinesIdQuery(), "Breakpoint Count By Source Guidelines Id", queryFilters);
        }

        /// <summary>
        /// Counts expert rules that reference specifications with the given guidelinesid (source/listitem id).
        /// Used when deleting a source to validate no expert rules depend on it.
        /// </summary>
        public async Task<int> GetExpertRuleCountBySourceGuidelinesIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run expert rule count by source guidelines id query", "CodingRepository", "GetExpertRuleCountBySourceGuidelinesIdAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new ExpertRuleCountBySourceGuidelinesIdQuery(), "Expert Rule Count By Source Guidelines Id", queryFilters);
        }
    }
}
