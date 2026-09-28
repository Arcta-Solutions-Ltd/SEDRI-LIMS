using arc.common.Models.Coding;
using arc.common.Models.Export;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public interface IExportProfileRepository
    {
        /// <summary>
        /// Retrieves export profile options for filter dropdowns (Key = id, Text = name).
        /// </summary>
        Task<IEnumerable<OptionsConfig>> GetExportProfileOptionsForListAsync();

        Task<IEnumerable<ExportProfileModel>> GetExportProfileListAsync(QueryFilterConfig parameters);
        Task<int> AddExportProfileAsync(string dataToSave);
        Task<int> EditExportProfileAsync(string dataToSave);
        Task<ExportProfileModel> EditExportProfileQueryAsync(QueryFilterConfig queryFilters);
        Task<List<ExportProfileFieldModel>> ExportProfileRecordViewQueryAsync(QueryFilterConfig queryFilters);

        Task DeleteExportProfileAsync(string id);
    }
}
