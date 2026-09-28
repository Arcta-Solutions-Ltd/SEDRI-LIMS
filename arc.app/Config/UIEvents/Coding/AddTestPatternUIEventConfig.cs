using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddTestPatternUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addtestpatternuievent',
                        description: 'Add Test Pattern',
                        type: 'form',
                        action: 'addtestpatternform'
                    }";

            return newEvent;
        }
    }
}
