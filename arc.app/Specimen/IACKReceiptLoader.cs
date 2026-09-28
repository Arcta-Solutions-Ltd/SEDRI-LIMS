using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Specimen
{
    public interface IACKReceiptLoader
    {
        Task<string> LoadDataAsync(QueryFilterConfig queryFilter, TokenInfoModel token);
    }
}
