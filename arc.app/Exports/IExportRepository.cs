using arc.common;
using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public interface IExportRepository
    {
        Task<IEnumerable<IdModel>> RunExportAsync(QueryFilterConfig queryFilter, TokenInfoModel token);
        Task<List<string>> ExportRunAsync(QueryFilterConfig queryFilter, TokenInfoModel token, List<FieldConfig> fieldConfigs);
        Task<List<WhonetAntibiotic>> AstExportAsync(QueryFilterConfig queryFilter, TokenInfoModel token);
        Task<List<ExportComment>> ExportCommentAsync(QueryFilterConfig filter, TokenInfoModel token);
    }
}
