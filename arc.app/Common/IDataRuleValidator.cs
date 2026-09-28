using arc.common.Models;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IDataRuleValidator
    {
        Task<string> ValidateMessageAsync(string message, EventConfig eventData, TokenInfoModel token);
    }
}
