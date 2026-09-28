using arc.app.Config.Workflows;
using arc.domain.Configuration.WorkflowsConfig;
using Newtonsoft.Json;

namespace arc.app.Config;

/// <summary>
/// Factory class for creating workflow configurations based on a workflow name.
/// Implements the <see cref="IWorkflowConfigFactory"/> interface.
/// </summary>
public class WorkflowConfigFactory : IWorkflowConfigFactory
{
    /// <summary>
    /// Retrieves the workflow configuration corresponding to the specified workflow name.
    /// </summary>
    /// <param name="workflowName">The name of the workflow to retrieve.</param>
    /// <returns>
    /// The <see cref="WorkflowConfig"/> object containing the workflow configuration data,
    /// or null if the workflow name is not recognized.
    /// </returns>
    public WorkflowConfig GetWorkflow(string workflowName)
    {
        var workflow = workflowName.ToLower() switch
        {
            "instrumenterrorworkflow" => new InstrumentErrorWorkflow().Get(),
            "instrumentresultworkflow" => new InstrumentResultWorkflow().Get(),
            "iqctestworkflow" => new IqcTestWorkflow().Get(),
            "specimendefault" => new SpecimenDefaultWorkflow().Get(),
            "unapprovedreportworkflow" => new UnapprovedReportWorkflow().Get(),
            _ => null,
        };

        return JsonConvert.DeserializeObject<WorkflowConfig>(workflow);
    }
}
