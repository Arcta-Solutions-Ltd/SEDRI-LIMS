using arc.app.Common;
using arc.app.Exports;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Repository for export profile mapping data access. Handles loading and upserting
    /// the JSON or XML structural mapper persisted alongside an export profile.
    /// </summary>
    public class ExportProfileMappingRepository : IExportProfileMappingRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportProfileMappingRepository"/> class.
        /// </summary>
        /// <param name="sqlCommand">The SQL command runner.</param>
        /// <param name="sqlQuery">The SQL query runner.</param>
        /// <param name="logWriter">The log writer used for diagnostic information.</param>
        public ExportProfileMappingRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<ExportProfileMappingModel?> GetByProfileIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run export profile mapping by profile id query", nameof(ExportProfileMappingRepository), nameof(GetByProfileIdAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new ExportProfileMappingByProfileIdQuery(), "Export Profile Mapping By Profile Id", queryFilters);
        }

        /// <inheritdoc />
        public async Task<int> SaveAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<ExportProfileMappingModel>(dataToSave, new JsonBooleanConverter());
            _logWriter.LogInfo($"Run save export profile mapping command for profile {data?.ExportProfileId}", nameof(ExportProfileMappingRepository), nameof(SaveAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddExportProfileMappingCommand(), "Save Export Profile Mapping", data, _logWriter);
        }
    }
}
