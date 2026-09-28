using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class WrightsStainTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'wrightsstaintestuievent',
                        description: 'Wrights Stain Test',
                        type: 'form',
                        action: 'wrightsstaintestform'
                    }";

            return newEvent;
        }
    }
}
