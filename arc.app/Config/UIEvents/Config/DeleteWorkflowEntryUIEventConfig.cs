using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteWorkflowEntryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteworkflowentryuievent',
                        description: 'Delete workflow entry',
                        type: 'form',
                        action: 'deleteworkflowentryform'
                    }";

            return newEvent;
        }
    }
}
