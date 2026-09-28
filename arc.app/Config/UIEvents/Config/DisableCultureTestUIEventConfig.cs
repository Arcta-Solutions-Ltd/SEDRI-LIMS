using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DisableCultureTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'disableculturetestuievent',
                        description: 'Disable culture test',
                        type: 'form',
                        action: 'disableculturetestform'
                    }";

            return newEvent;
        }
    }
}
