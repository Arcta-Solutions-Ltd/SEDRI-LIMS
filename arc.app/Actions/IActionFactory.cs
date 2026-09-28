using arc.common.Models;

namespace arc.app.Actions
{
    public interface IActionFactory
    {
        IAction Get(string actionName, TokenInfoModel token);
    }
}
