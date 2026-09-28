using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public interface IExportProfileQueryHandler
    {
        Task<string> GetProfileFieldsForEditAsync(QueryFilterConfig queryFilters);
    }
}
