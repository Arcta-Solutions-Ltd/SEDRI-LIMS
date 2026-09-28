using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Repository interface for export profile mapping data access.
    /// </summary>
    public interface IExportProfileMappingRepository
    {
        /// <summary>
        /// Gets the mapping (JSON or XML structural mapper) saved against an export profile.
        /// </summary>
        /// <param name="queryFilters">Filter that must contain ExportProfileId.</param>
        /// <returns>The persisted mapping, or null when no mapping has been saved yet.</returns>
        Task<ExportProfileMappingModel?> GetByProfileIdAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Upserts the single mapping row for the supplied export profile.
        /// </summary>
        /// <param name="dataToSave">JSON serialized <see cref="ExportProfileMappingModel"/>.</param>
        /// <returns>The id of the upserted mapping row.</returns>
        Task<int> SaveAsync(string dataToSave);
    }
}
