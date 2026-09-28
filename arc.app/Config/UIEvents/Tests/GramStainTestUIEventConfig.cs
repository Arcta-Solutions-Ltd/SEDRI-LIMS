using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class GramStainTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'gramstaintestuievent',
                        description: 'Gram Stain Test',
                        type: 'form',
                        action: 'gramstaintestform'
                    }";

            return newEvent;
        }
    }
}
