using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CreateSpecimenRequestForPatientUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'createspecimenrequestforpatientuievent',
                        description: 'Create a new specimen request',
                        type: 'form',
                        action: 'createspecimenrequestforpatientform'
                    }";

            return newEvent;
        }
    }
}
