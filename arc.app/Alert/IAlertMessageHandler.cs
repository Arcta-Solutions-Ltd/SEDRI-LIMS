using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Alert
{
    public interface IAlertMessageHandler
    {
        Task<List<AlertMessageModel>> GetMessagesAsync(QueryFilterConfig parameters);
    }
}
