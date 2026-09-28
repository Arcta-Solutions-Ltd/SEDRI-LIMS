using arc.app.Common;
using arc.app.Exports;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    public class ExportProfileRepository : IExportProfileRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        public ExportProfileRepository(ISqlCommand sqlCommand,ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
        }

        public async Task<int> AddExportProfileAsync(string dataToSave)
        {
            var data = GetExportProfileData(dataToSave);
            _logWriter.LogInfo("Run add export profile command", "ExportProfileRepository", "AddExportProfileAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddExportProfileCommand(), "Insert Export Profile", data);
        }

        public async Task<int> EditExportProfileAsync(string dataToSave)
        {
            var data = GetExportProfileData(dataToSave);
            _logWriter.LogInfo("Run edit export profile command", "ExportProfileRepository", "EditExportProfileAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new EditExportProfileCommand(), "Edit Export Profile", data);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<OptionsConfig>> GetExportProfileOptionsForListAsync()
        {
            _logWriter.LogInfo("Run export profile options for list query", "ExportProfileRepository", "GetExportProfileOptionsForListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ExportProfileOptionsForListQuery(), "Export Profile Options For List", new QueryFilterConfig());
        }

        public async Task<IEnumerable<ExportProfileModel>> GetExportProfileListAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("Run get export profile query", "ExportProfileRepository", "GetExportProfileListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ExportProfileListQuery(),"Get ExportProfile List", parameters);
        }

        public async Task<ExportProfileModel> EditExportProfileQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get export profile query", "ExportProfileRepository", "EditExportProfileQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new EditExportProfileQuery(), "Edit Export Profile Query", queryFilters);
        }

        public async Task<List<ExportProfileFieldModel>> ExportProfileRecordViewQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get export profile field query", "ExportProfileRepository", "ExportProfileRecordViewQuery");
            return await _sqlQuery.QueryReturningTypeAsync(new ExportProfileRecordViewQuery(), "Export Profile Record View Query", queryFilters);
        }

        public async Task DeleteExportProfileAsync(string id)
        {
            _logWriter.LogInfo("Run delete export profile command", "ExportProfileRepository", "DeleteExportProfileAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteExportProfileCommand(), "Delete Export profile", id);
        }

        private ExportProfileModel GetExportProfileData(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<ExportProfileModel>(dataToSave, new JsonBooleanConverter());
            return data;
        }
    }
}
