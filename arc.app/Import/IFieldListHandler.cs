using arc.common.Models;
using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Import
{
    public interface IFieldListHandler
    {
        Task<List<OptionsConfig>> GetFieldsAsync(TokenInfoModel token, string form);
    }
}
