using arc.domain.Configuration.WorkflowsConfig;

namespace arc.app.Config
{
    public interface IWorkflowConfigFactory
    {
        WorkflowConfig GetWorkflow(string workflowName);
    }
}
