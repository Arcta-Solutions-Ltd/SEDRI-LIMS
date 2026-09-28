using arc.app.Common;
using arc.app.SystemConfig;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.ReportsConfig;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    public class ReportConfigRepository : IReportConfigRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        public ReportConfigRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
        }

        public async Task<int> AddNewReportAsync(FullReportConfig newReport)
        {
            _logWriter.LogInfo("Add new report command", "ReportConfigRepository", "DeleteSectionAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddNewReportCommand(), "Add a new report", newReport);
        }

        public async Task<int> DeleteReportAsync(FullReportConfig newReport)
        {
            _logWriter.LogInfo("Delete report command", "ReportConfigRepository", "DeleteSectionAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new DeleteReportCommand(), "Delete report", newReport);
        }

        public async Task<int> AddNewSectionAsync(ReportSectionConfig newSection)
        {
            _logWriter.LogInfo("Add new section command", "ReportConfigRepository", "DeleteSectionAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddSectionCommand(), "Add new report section", newSection);
        }

        public async Task<int> EditSectionAsync(ReportSectionConfig section)
        {
            _logWriter.LogInfo("Edit section command", "ReportConfigRepository", "DeleteSectionAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new EditSectionCommand(), "Edit existing report section", section);
        }

        public async Task DeleteSectionAsync(string id)
        {
            _logWriter.LogInfo("Run delete section command", "ReportConfigRepository", "DeleteSectionAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteSectionCommand(), "Delete Section", id);
        }

        /// <summary>
        /// Saves every report designer change in a single transaction owned by the command.
        /// </summary>
        /// <param name="config">The change set produced by the designer.</param>
        /// <returns>A description of every configuration record written, so the designer can reconcile renames.</returns>
        public async Task<ReportDesignerSaveResultModel> SaveReportDesignerConfigAsync(SaveReportDesignerConfigModel config)
        {
            _logWriter.LogInfo(
                $"Save report designer config command for '{config?.Name}' with {config?.ChangedCustomFormats?.Count ?? 0} format(s) and {config?.ChangedSectionDefinitions?.Count ?? 0} section(s)",
                "ReportConfigRepository", "SaveReportDesignerConfigAsync");
            return await _sqlCommand.CommandWithTypeReturningTypeAsync(new SaveReportDesignerConfigCommand(), "Save report designer configuration", config, _logWriter);
        }
    }
}
