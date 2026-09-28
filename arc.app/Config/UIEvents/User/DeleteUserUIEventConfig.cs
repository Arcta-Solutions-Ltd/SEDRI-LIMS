using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteUserUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteuseruievent',
                        description: 'Delete a user',
                        type: 'form',
                        action: 'deleteuserform'
                    }";

            return newEvent;
        }
    }
}
