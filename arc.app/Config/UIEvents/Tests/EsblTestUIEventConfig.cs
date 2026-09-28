using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EsblTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'esbltestuievent',
                        description: 'Esbl Test',
                        type: 'form',
                        action: 'esbltestform'
                    }";

            return newEvent;
        }
    }
}
