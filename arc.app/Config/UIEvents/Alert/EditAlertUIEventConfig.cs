using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditAlertUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editalertuievent',
                        description: 'Edit Alert',
                        type: 'form',
                        action: 'editalertform'
                    }";

            return newEvent;
        }
    }
}
