using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditSectionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editsectionuievent',
                        description: 'Edit a section',
                        type: 'form',
                        action: 'editsectionform'
                    }";

            return newEvent;
        }
    }
}
