using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DeleteWorkflowEntryQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'DeleteWorkflowEntryQuery', 'Type': 'Config', Translate: true}";
        }
    }
}
