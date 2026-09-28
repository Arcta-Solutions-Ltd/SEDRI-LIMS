using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteTestPatternUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletetestpatternuievent',
                        description: 'Delete Test Pattern',
                        type: 'form',
                        action: 'deletetestpatternform'
                    }";

            return newEvent;
        }
    }
}
