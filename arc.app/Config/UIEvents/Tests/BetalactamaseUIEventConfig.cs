using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class BetalactamaseUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'betalactamasetestuievent',
                        description: 'Betalactamase Test',
                        type: 'form',
                        action: 'betalactamasetestform'
                    }";

            return newEvent;
        }
    }
}
