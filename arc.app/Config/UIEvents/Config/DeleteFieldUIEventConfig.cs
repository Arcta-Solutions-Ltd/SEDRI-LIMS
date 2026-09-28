using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteFieldUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletefielduievent',
                        description: 'Delete field',
                        type: 'form',
                        action: 'deletefieldform'
                    }";

            return newEvent;
        }
    }
}
