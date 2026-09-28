using arc.common.Models;
using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    public interface IProfileListHandler
    {
        Task<List<OptionsConfig>> GetListAsync(TokenInfoModel token);
    }
}
