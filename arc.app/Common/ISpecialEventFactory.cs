using arc.common;
using arc.common.Models;

namespace arc.app.Common
{
    public interface ISpecialEventFactory
    {
        IRun GetEvent(string eventName, TokenInfoModel token);
    }
}
