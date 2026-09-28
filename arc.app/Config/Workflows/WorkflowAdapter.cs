using arc.app.SystemConfig;
using arc.domain.Configuration.WorkflowsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Workflows;

/// <summary>
/// Provides methods to retrieve and adapt workflow configurations.
/// </summary>
public class WorkflowAdapter : IWorkflowAdapter
{
    private readonly IWorkflowConfigFactory _workflowFactory;
    private readonly IConfigRepository _configRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowAdapter"/> class.
    /// </summary>
    /// <param name="workflowFactory">The factory used to obtain default workflow configurations.</param>
    /// <param name="configRepository">The repository for retrieving workflow configuration records.</param>
    public WorkflowAdapter(IWorkflowConfigFactory workflowFactory, IConfigRepository configRepository)
    {
        _workflowFactory = workflowFactory;
        _configRepository = configRepository;
    }

    /// <summary>
    /// Retrieves the workflow configuration for the specified workflow.
    /// </summary>
    /// <param name="workflowName">The name of the workflow to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="WorkflowConfig"/> instance
    /// for the workflow. If no custom configuration is found or the configuration is empty, a default workflow is returned.
    /// </returns>
    public async Task<WorkflowConfig> GetWorkflowAsync(string workflowName)
    {
        var parameters = new QueryFilterConfig
        {
            Parameters =
            [
                new() { Key = "ConfigName", Value = workflowName }
            ]
        };

        var configRecord = await _configRepository.SingleConfigByNameAsync(parameters);

        var workflowDef = configRecord.Contents == null || configRecord.Contents == "{}"
            ? _workflowFactory.GetWorkflow(workflowName)
            : JsonConvert.DeserializeObject<WorkflowConfig>(configRecord.Contents);

        return workflowDef;
    }
}
