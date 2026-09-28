using arc.common.Models;
using arc.domain.Configuration.WorkflowsConfig;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Defines a contract for retrieving workflow configurations based on a specimen ID.
/// </summary>
public interface IWorkflowFinder
{
    /// <summary>
    /// Asynchronously retrieves the current workflow configuration based on the provided specimen, specimen type, and laboratory parameters.
    /// </summary>
    /// <param name="specimenId">The identifier for the specimen.</param>
    /// <param name="specimenTypeId">The identifier for the specimen type.</param>
    /// <param name="laboratoryId">The identifier for the laboratory.</param>
    /// <param name="token">The token information for authentication and authorization.</param>
    /// <returns>
    /// A <see cref="Task{WorkflowConfig}"/> representing the asynchronous operation that returns the current workflow configuration.
    /// </returns>
    Task<WorkflowConfig> GetCurrentWorkflowAsync(int specimenId, int specimenTypeId, int laboratoryId, TokenInfoModel token);
}
