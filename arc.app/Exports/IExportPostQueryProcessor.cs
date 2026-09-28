using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Transforms the raw pipe-delimited export rows returned by the query into their final display
    /// form (list-item translation, hierarchy/WHONET/comment expansion, grids, value mappings).
    /// </summary>
    public interface IExportPostQueryProcessor
    {
        /// <summary>
        /// Processes the raw export rows and returns the final rows plus an aligned set of column keys.
        /// </summary>
        /// <param name="data">The raw rows (index 0 is the header) as returned by the export query.</param>
        /// <param name="exportProfile">The ordered export profile fields (including any appended hidden id columns).</param>
        /// <param name="fieldConfigs">The field configurations aligned with <paramref name="exportProfile"/>.</param>
        /// <param name="queryFilters">The query filters for the export run.</param>
        /// <param name="token">The caller's token information.</param>
        /// <returns>The processed rows and the field-id keys aligned with each output column.</returns>
        Task<ExportProcessResult> Process(List<string> data, List<ExportProfileFieldModel> exportProfile, List<FieldConfig> fieldConfigs, QueryFilterConfig queryFilters, TokenInfoModel token);
    }
}
