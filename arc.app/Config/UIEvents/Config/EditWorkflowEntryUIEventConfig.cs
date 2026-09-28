using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditWorkflowEntryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editworkflowentryuievent',
                        description: 'Edit workflow entry',
                        type: 'form',
                        action: 'editworkflowentryform'
                    }";

            return newEvent;
        }
    }
}
