using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CultureTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'culturetestuievent',
                        description: 'Culture Tests',
                        type: 'form',
                        action: 'culturetestform',
                        nextButton: { show: false },
                        cancelButton: { buttonText: '@GenClo@' }
                    }";

            return newEvent;
        }
    }
}
