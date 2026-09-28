using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteStateUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletestateuievent',
                        description: 'Delete state',
                        type: 'form',
                        action: 'deletestateform'
                    }";

            return newEvent;
        }
    }
}
