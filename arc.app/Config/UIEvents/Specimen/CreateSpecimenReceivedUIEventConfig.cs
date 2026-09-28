using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CreateSpecimenReceivedUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'createspecimenreceived',
                        description: 'Record a specimen has been received',
                        type: 'form',
                        action: 'createspecimenreceivedform'
                    }";

            return newEvent;
        }
    }
}
