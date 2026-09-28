using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class SpecimenCancelRequestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'specimencancelrequestuievent',
                        description: 'Cancel Specimen Request',
                        type: 'form',
                        action: 'specimencancelrequestform'
                    }";

            return newEvent;
        }
    }
}
