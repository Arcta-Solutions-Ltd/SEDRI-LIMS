using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Handler responsible for loading the existing mapping for an export profile and the
    /// classified field options the Manage Mapping editor needs to enforce constraints.
    /// </summary>
    public interface IExportProfileMappingHandler
    {
        /// <summary>
        /// Loads the export profile mapping (or a default empty one) plus the classified
        /// field options derived from the profile's saved fields.
        /// </summary>
        /// <param name="queryFilters">Filter that must contain ExportProfileId.</param>
        /// <param name="token">The current user token (used for translation).</param>
        /// <returns>A populated <see cref="ExportProfileMappingViewModel"/>.</returns>
        Task<ExportProfileMappingViewModel> LoadAsync(QueryFilterConfig queryFilters, TokenInfoModel token);

        /// <summary>
        /// Persists a mapping update for the supplied export profile.
        /// </summary>
        /// <param name="request">The mapping data submitted by the editor.</param>
        /// <returns>The id of the persisted row.</returns>
        Task<int> SaveAsync(SaveExportProfileMappingRequest request);
    }
}
