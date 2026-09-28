using arc.domain.Configuration.WorkflowsConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Workflows;

/// <summary>
/// Defines an interface for managing and retrieving workflow configurations.
/// </summary>
public interface IWorkflowAdapter
{
    /// <summary>
    /// Asynchronously retrieves a workflow configuration based on its name.
    /// </summary>
    /// <param name="workflowName">The name of the workflow to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains
    /// the <see cref="WorkflowConfig"/> object with the workflow configuration data.
    /// </returns>
    Task<WorkflowConfig> GetWorkflowAsync(string workflowName);
}

