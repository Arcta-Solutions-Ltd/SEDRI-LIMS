using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditSettingUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editsettinguievent',
                        description: 'Edit Setting',
                        type: 'form',
                        action: 'editsettingform'
                    }";

            return newEvent;
        }
    }
}
