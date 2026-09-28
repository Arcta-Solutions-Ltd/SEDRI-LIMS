using arc.app.Common;
using arc.app.Exports;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Repository for export schedule data access.
    /// Handles CRUD operations and queries for scheduled exports.
    /// </summary>
    public class ExportScheduleRepository : IExportScheduleRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportScheduleRepository"/> class.
        /// </summary>
        public ExportScheduleRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<List<ExportScheduleListModel>> GetSchedulesByProfileIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run export schedule list by profile id query", nameof(ExportScheduleRepository), nameof(GetSchedulesByProfileIdAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new ExportScheduleListByProfileIdQuery(), "Export Schedule List", queryFilters);
        }

        /// <inheritdoc />
        public async Task<ExportScheduleModel?> GetByIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run export schedule by id query", nameof(ExportScheduleRepository), nameof(GetByIdAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new ExportScheduleByIdQuery(), "Export Schedule By Id", queryFilters);
        }

        /// <inheritdoc />
        public async Task<List<ExportScheduleModel>> GetEnabledSchedulesAsync()
        {
            _logWriter.LogInfo("Run get enabled export schedules query", nameof(ExportScheduleRepository), nameof(GetEnabledSchedulesAsync));
            var parameters = new QueryFilterConfig();
            return await _sqlQuery.QueryReturningTypeAsync(new GetEnabledExportSchedulesQuery(), "Get Enabled Export Schedules", parameters);
        }

        /// <inheritdoc />
        public async Task<int> AddExportScheduleAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<ExportScheduleModel>(dataToSave, new JsonBooleanConverter());
            _logWriter.LogInfo("Run add export schedule command", nameof(ExportScheduleRepository), nameof(AddExportScheduleAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddExportScheduleCommand(), "Insert Export Schedule", data, _logWriter);
        }

        /// <inheritdoc />
        public async Task<int> EditExportScheduleAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<ExportScheduleModel>(dataToSave, new JsonBooleanConverter());
            _logWriter.LogInfo("Run edit export schedule command", nameof(ExportScheduleRepository), nameof(EditExportScheduleAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new EditExportScheduleCommand(), "Edit Export Schedule", data, _logWriter);
        }

        /// <inheritdoc />
        public async Task DeleteExportScheduleAsync(string id)
        {
            _logWriter.LogInfo("Run delete export schedule command", nameof(ExportScheduleRepository), nameof(DeleteExportScheduleAsync));
            await _sqlCommand.CarryOutCommandAsync(new DeleteExportScheduleCommand(), "Delete Export Schedule", id);
        }

        /// <inheritdoc />
        public async Task<System.DateTime?> GetLastRunForScheduleAsync(int scheduleId)
        {
            var parameters = new QueryFilterConfig().AddInteger("ExportScheduleId", scheduleId);
            return await _sqlQuery.QueryReturningTypeAsync(new GetLastRunForScheduleQuery(), "Get Last Run For Schedule", parameters);
        }

        /// <inheritdoc />
        public async Task<bool> ScheduleNameExistsAsync(int exportProfileId, string name, int? excludeScheduleId = null)
        {
            var parameters = new QueryFilterConfig
            {
                Parameters = new List<QueryValuesConfig>
                {
                    new QueryValuesConfig { Key = "ExportProfileId", Value = exportProfileId.ToString() },
                    new QueryValuesConfig { Key = "Name", Value = name ?? "" },
                    new QueryValuesConfig { Key = "ExcludeScheduleId", Value = excludeScheduleId?.ToString() ?? "0" }
                }
            };
            return await _sqlQuery.QueryReturningTypeAsync(new ExportScheduleNameExistsQuery(), "Export Schedule Name Exists", parameters);
        }
    }
}
