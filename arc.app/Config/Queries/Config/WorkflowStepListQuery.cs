using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class WorkflowStepListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'WorkflowStepListQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}
