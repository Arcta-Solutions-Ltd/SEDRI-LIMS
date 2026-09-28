using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DipstickTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'dipsticktestuievent',
                        description: 'Dipstick Test',
                        type: 'form',
                        action: 'dipsticktestform'
                    }";

            return newEvent;
        }
    }
}
