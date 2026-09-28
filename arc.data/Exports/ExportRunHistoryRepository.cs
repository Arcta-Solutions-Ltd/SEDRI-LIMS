using arc.app.Common;
using arc.app.Exports;
using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Repository for export run history data access.
    /// </summary>
    public class ExportRunHistoryRepository : IExportRunHistoryRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        public ExportRunHistoryRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<int> AddExportRunAsync(ExportRunRequestModel exportRunRequest)
        {
            _logWriter.LogInfo("Run add export run history command", "ExportRunHistoryRepository", "AddExportRunAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddExportRunHistoryCommand(), "Insert Export Run", exportRunRequest);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ExportHistoryModel>> GetExportHistoryListAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("Run get export history list query", "ExportRunHistoryRepository", "GetExportHistoryListAsync");
            var exportProfileIdParam = parameters?.Parameters?.FirstOrDefault(p => string.Equals(p.Key, "exportprofileid", System.StringComparison.OrdinalIgnoreCase));
            if (exportProfileIdParam != null && !string.IsNullOrWhiteSpace(exportProfileIdParam.Value))
            {
                var idCount = exportProfileIdParam.Value.Split(',').Count(id => !string.IsNullOrWhiteSpace(id.Trim()));
                _logWriter.LogInfo($"Export history list filtered by {idCount} export profile(s): {exportProfileIdParam.Value}", "ExportRunHistoryRepository", "GetExportHistoryListAsync");
            }
            return await _sqlQuery.QueryReturningTypeAsync(new ExportHistoryListQuery(), "Get Export History List", parameters);
        }

        /// <inheritdoc />
        public async Task<ExportHistoryModel?> GetExportHistoryByIdAsync(int id)
        {
            _logWriter.LogInfo("Run get export history by id query", "ExportRunHistoryRepository", "GetExportHistoryByIdAsync");
            var parameters = new QueryFilterConfig().AddInteger("id", id);
            return await _sqlQuery.QueryReturningTypeAsync(new ExportHistoryByIdQuery(), "Get Export History By Id", parameters);
        }
    }
}
