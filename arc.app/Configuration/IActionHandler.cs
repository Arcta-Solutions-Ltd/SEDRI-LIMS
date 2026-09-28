using arc.common;
using arc.common.Models;
using arc.domain.Configuration.WorkflowsConfig;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Executes workflow-configured side-effect actions after state transitions.
    /// </summary>
    public interface IActionHandler
    {
        /// <summary>
        /// Runs the action for the workflow transition. Query parameters are built from the resolved record id
        /// plus <paramref name="action"/>.<see cref="ActionConditionConfig.Parameters"/> (see <see cref="ActionHandler"/>).
        /// </summary>
        Task CarryOutAction(ActionConditionConfig action, string message, IdModel recordId, TokenInfoModel token);
    }
}
