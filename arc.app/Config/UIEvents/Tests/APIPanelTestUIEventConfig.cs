using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class APIPanelTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'apipaneltestuievent',
                        description: 'API Panel Test',
                        type: 'form',
                        action: 'apipaneltestform'
                    }";

            return newEvent;
        }
    }
}
