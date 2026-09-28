using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class WetPrepTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'wetpreptestuievent',
                        description: 'Wet Prep Test',
                        type: 'form',
                        action: 'wetpreptestform'
                    }";

            return newEvent;
        }
    }
}
