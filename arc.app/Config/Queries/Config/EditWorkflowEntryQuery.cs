using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditWorkflowEntryQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'EditWorkflowEntryQuery', 'Type': 'Config'}";
        }
    }
}
