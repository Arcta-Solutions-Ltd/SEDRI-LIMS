using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class TestSelectionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'testselectionuievent',
                        description: 'Test Selection',
                        type: 'form',
                        action: 'testselectionform'
                    }";

            return newEvent;
        }
    }
}
