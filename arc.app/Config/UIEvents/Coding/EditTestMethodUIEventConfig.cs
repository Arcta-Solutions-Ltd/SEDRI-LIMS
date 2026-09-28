using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditTestMethodUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'edittestmethoduievent',
                        description: 'Add Test Method',
                        type: 'form',
                        action: 'edittestmethodform'
                    }";

            return newEvent;
        }
    }
}
