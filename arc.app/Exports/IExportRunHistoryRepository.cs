using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Repository for export run history operations.
    /// </summary>
    public interface IExportRunHistoryRepository
    {
        /// <summary>
        /// Adds an export run history record with the given criteria and optional file attachment.
        /// </summary>
        Task<int> AddExportRunAsync(ExportRunRequestModel exportRunRequest);

        /// <summary>
        /// Gets the list of export history records for the list view.
        /// </summary>
        Task<IEnumerable<ExportHistoryModel>> GetExportHistoryListAsync(QueryFilterConfig parameters);

        /// <summary>
        /// Gets a single export history record by id for the record view.
        /// </summary>
        Task<ExportHistoryModel?> GetExportHistoryByIdAsync(int id);
    }
}
