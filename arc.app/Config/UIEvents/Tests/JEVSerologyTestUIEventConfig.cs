using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class JEVSerologyTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'jevserologytestuievent',
                        description: 'JEV Serology Test',
                        type: 'form',
                        action: 'jevserologytestform'
                    }";

            return newEvent;
        }
    }
}
