using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class IndiaInkTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'indiainktestuievent',
                        description: 'India Ink Test',
                        type: 'form',
                        action: 'indiainktestform'
                    }";

            return newEvent;
        }
    }
}
