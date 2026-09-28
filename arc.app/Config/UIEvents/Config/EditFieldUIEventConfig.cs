using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditFieldUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editfielduievent',
                        description: 'Edit field',
                        type: 'form',
                        action: 'editfieldform'
                    }";

            return newEvent;
        }
    }
}
