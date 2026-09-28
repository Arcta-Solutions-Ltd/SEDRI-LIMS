using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteSettingUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletesettinguievent',
                        description: 'Delete Setting',
                        type: 'form',
                        action: 'deletesettingform'
                    }";

            return newEvent;
        }
    }
}
