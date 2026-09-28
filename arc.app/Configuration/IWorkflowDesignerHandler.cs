using arc.common.Models.Common;
using arc.common.Models.Config;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Loads the workflow document and supporting lookups the workflow designer renders.
/// </summary>
public interface IWorkflowDesignerHandler
{
    /// <summary>
    /// Loads one workflow for the designer.
    /// </summary>
    /// <param name="workflowId">The configs identity and name of the workflow, as sent by the designer.</param>
    /// <returns>The workflow document and the reference data needed to display it.</returns>
    Task<WorkflowDesignerConfigModel> GetWorkflowDesignerConfigurationAsync(IdAndNameModel workflowId);
}
