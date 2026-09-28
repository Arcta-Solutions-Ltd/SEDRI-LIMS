using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteOrganisationUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteorganisationuievent',
                        description: 'Delete an organisation',
                        type: 'form',
                        action: 'deleteorganisationform'
                    }";

            return newEvent;
        }
    }
}
