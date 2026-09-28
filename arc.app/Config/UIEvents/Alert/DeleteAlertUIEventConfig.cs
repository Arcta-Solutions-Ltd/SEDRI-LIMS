using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteAlertUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletealertuievent',
                        description: 'Delete Alert',
                        type: 'form',
                        action: 'deletealertform'
                    }";

            return newEvent;
        }
    }
}
