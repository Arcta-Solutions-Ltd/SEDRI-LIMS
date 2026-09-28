using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.SystemConfig
{
    public interface IListFilterHandler
    {
        Task<List<OptionsConfig>> GetGridFieldListAsync();
    }
}
