using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class KOHPrepTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'kohpreptestuievent',
                        description: 'KOH Prep Test',
                        type: 'form',
                        action: 'kohpreptestform'
                    }";

            return newEvent;
        }
    }
}
