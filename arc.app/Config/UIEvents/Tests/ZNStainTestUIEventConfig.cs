using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ZNStainTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'znstaintestuievent',
                        description: 'ZN Stain Test',
                        type: 'form',
                        action: 'znstaintestform'
                    }";

            return newEvent;
        }
    }
}
