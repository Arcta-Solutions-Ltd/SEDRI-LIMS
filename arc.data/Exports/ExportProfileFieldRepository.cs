using arc.app.Common;
using arc.app.Exports;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    public class ExportProfileFieldRepository : IExportProfileFieldRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        public ExportProfileFieldRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
        }
        public async Task<int> AddExportProfileFieldAsync(ExportProfileFieldModel  exportProfileField)
        {
            _logWriter.LogInfo("Run add export profile field command", "ExportProfileFieldRepository", "AddExportProfileFieldAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddExportProfileFieldCommand(), "Insert Export Profile Field", exportProfileField);
        }

        public async Task<IEnumerable<string>> GetAllFieldMappingsAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("Run get export field mappings query", "ExportProfileFieldRepository", "GetAllFieldMappingsAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new GetExportFieldMappings(), "Get All Export Field Mappings", parameters);
        }

        public Task<IEnumerable<ExportProfileFieldModel>> GetAsync(QueryFilterConfig parameters)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteExportProfileFieldAsync(string id)
        {
            _logWriter.LogInfo("Run delete export profile field command", "ExportProfileFieldRepository", "DeleteExportProfileFieldAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteExportProfileFieldCommand(), "Delete Export profile", id);
        }

        public async Task<IEnumerable<ExportProfileFieldModel>> GetByProfileIdAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("Run get export profile query", "ExportProfileFieldRepository", "GetByProfileIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ExportProfileFieldsForProfileIdQuery(), "Edit Export Profile Query", parameters);
        }

        public async Task UpdateAsync(string id, ExportProfileFieldModel exportProfileFieldModel)
        {
            _logWriter.LogInfo("Run update export profile field command", "ExportProfileFieldRepository", "UpdateAsync");
            await _sqlCommand.CommandWithTypeQueryAsync(new UpdateExportProfileFieldCommand(), "Update Profile field", exportProfileFieldModel);
        }

        public async Task<ExportProfileFieldModel> GetByIdAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("Run get export profile query", "ExportProfileFieldRepository", "GetByIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ExportProfileFieldByIdQuery(), "Get a profile field by id", parameters);
        }
    }
}
