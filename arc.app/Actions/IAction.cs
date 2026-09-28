using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Actions
{
    public interface IAction
    {
        Task Do(QueryFilterConfig queryFilter, TokenInfoModel token);
    }
}
