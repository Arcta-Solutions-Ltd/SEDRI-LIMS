using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteInstrumentProfileUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteinstrumentprofileuievent',
                        description: '@InsDelC@',
                        type: 'form',
                        action: 'deleteinstrumentprofileform'
                    }";

            return newEvent;
        }
    }
}
