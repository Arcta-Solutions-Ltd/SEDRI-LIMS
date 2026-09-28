using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DirectTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'directtestuievent',
                        description: 'Direct Tests',
                        type: 'form',
                        action: 'directtestform',
                        nextButton: { show: false },
                        cancelButton: { buttonText: '@GenClo@' }
                    }";

            return newEvent;
        }
    }
}
