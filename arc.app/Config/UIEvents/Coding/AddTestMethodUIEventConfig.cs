using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddTestMethodUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addtestmethoduievent',
                        description: 'Add Test Method',
                        type: 'form',
                        action: 'addtestmethodform'
                    }";

            return newEvent;
        }
    }
}
