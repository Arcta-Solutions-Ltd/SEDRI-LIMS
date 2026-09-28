using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditOrganisationUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editorganisationuievent',
                        description: 'Edit Organisation',
                        type: 'form',
                        action: 'editorganisationform'
                    }";

            return newEvent;
        }
    }
}
