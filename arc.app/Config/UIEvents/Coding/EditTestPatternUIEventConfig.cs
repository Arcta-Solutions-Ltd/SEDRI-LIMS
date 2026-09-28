using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditTestPatternUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'edittestpatternuievent',
                        description: 'Edit Test Pattern',
                        type: 'form',
                        action: 'edittestpatternform'
                    }";

            return newEvent;
        }
    }
}
