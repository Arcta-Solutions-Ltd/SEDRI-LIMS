using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DisableDirectTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'disabledirecttestuievent',
                        description: 'Disable direct test',
                        type: 'form',
                        action: 'disabledirecttestform'
                    }";

            return newEvent;
        }
    }
}
