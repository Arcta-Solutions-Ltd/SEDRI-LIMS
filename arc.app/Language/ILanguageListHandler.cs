using arc.common.Models.Language;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Language
{
    public interface ILanguageListHandler
    {
        Task<List<LanguageListModel>> GetListAsync(QueryFilterConfig parameters);
    }
}
