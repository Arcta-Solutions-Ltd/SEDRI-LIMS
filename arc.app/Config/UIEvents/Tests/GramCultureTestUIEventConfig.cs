using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class GramCultureTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'gramculturetestuievent',
                        description: 'Gram Culture Test',
                        type: 'form',
                        action: 'gramculturetestform'
                    }";

            return newEvent;
        }
    }
}
