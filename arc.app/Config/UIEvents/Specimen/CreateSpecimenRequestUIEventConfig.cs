using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CreateSpecimenRequestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'createspecimenrequest',
                        description: 'Create a new specimen request',
                        type: 'form',
                        action: 'createspecimenrequestform'
                    }";

            return newEvent;
        }
    }
}
