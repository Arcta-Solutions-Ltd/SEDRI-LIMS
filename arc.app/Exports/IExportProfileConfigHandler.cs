using arc.common.Models;
using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public interface IExportProfileConfigHandler
    {
        Task<List<OptionsConfig>> GetFieldsAsync(TokenInfoModel token);
    }
}
