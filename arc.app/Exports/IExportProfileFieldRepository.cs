using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public interface IExportProfileFieldRepository
    {
        Task<IEnumerable<ExportProfileFieldModel>> GetAsync(QueryFilterConfig parameters);
        Task<IEnumerable<string>> GetAllFieldMappingsAsync(QueryFilterConfig parameters);
        Task<ExportProfileFieldModel> GetByIdAsync(QueryFilterConfig parameters);
        
        Task<IEnumerable<ExportProfileFieldModel>> GetByProfileIdAsync(QueryFilterConfig parameters);
        Task<int> AddExportProfileFieldAsync(ExportProfileFieldModel exportProfileField);

        Task DeleteExportProfileFieldAsync(string id);

        Task UpdateAsync(string id, ExportProfileFieldModel exportProfileFieldModel);

    }
}
