using arc.common.Models;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Specimen
{
    public interface ICultureList
    {
        Task<List<CultureListModel>> GetAsync(QueryFilterConfig parameters, TokenInfoModel token);
    }
}
