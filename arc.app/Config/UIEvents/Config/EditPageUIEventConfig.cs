using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditPageUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editpageuievent',
                        description: 'Edit page definition',
                        type: 'form',
                        action: 'editpageform'
                    }";

            return newEvent;
        }
    }
}
