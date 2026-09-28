using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddOrganisationUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addorganisationuievent',
                        description: 'Add Organisation',
                        type: 'form',
                        action: 'addorganisationform'
                    }";

            return newEvent;
        }
    }
}
