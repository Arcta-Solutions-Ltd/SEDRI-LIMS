using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CreateSpecimenReceivedForPatientUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'createspecimenreceivedforpatientuievent',
                        description: 'Record a specimen has been received',
                        type: 'form',
                        action: 'createspecimenreceivedforpatientform'
                    }";

            return newEvent;
        }
    }
}
